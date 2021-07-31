module AverageWeeklyMessagesChart

open System
open System.Globalization

open XPlot.Plotly

let averageWeeklyMessagesChart
    ({ Messages = messages
       Participants = participants }: Conversation.T)
    =
    let messagesBySender =
        messages |> Seq.groupBy (fun m -> m.SenderName)

    let messagesByDayOfWeek =
        messagesBySender
        |> Seq.map (fun (s, ms) -> (s, ms |> Seq.groupBy (fun m -> m.Timestamp.DayOfWeek)))

    let dayOfWeekCounts (dOfWeek: DayOfWeek) (messages: seq<Conversation.Message>) =
        messages
        |> Seq.distinctBy (fun m -> (m.Timestamp.Year, m.Timestamp.DayOfYear))
        |> Seq.filter (fun m -> m.Timestamp.DayOfWeek = dOfWeek)
        |> Seq.length

    let data =
        messagesByDayOfWeek
        |> Seq.map
            (fun (s, g) ->
                (s,
                 g
                 |> Seq.map
                     (fun (d, ms) ->
                         let messagesLength = ms |> Seq.length
                         let dayOfWeekCounts = dayOfWeekCounts d ms
                         (d, messagesLength / dayOfWeekCounts))))

    let bars =
        data
        |> Seq.map
            (fun (s, g) ->
                let x, y =
                    g |> Seq.sortBy fst |> Array.ofSeq |> Array.unzip

                Bar(
                    x =
                        (x
                         |> Array.map DateTimeFormatInfo.CurrentInfo.GetAbbreviatedDayName),
                    y = y,
                    name = s
                ))

    let title = participants |> String.concat ", "

    let stackedLayout = Layout(barmode = "stack")

    let chart =
        bars
        |> Chart.Plot
        |> Chart.WithLayout stackedLayout
        |> Chart.WithHeight 800
        |> Chart.WithWidth 1200
        |> Chart.WithTitle title

    chart
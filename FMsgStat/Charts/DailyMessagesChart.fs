module DailyMessagesChart

open XPlot.Plotly

let dailyMessagesChart
    ({ Messages = messages
       Participants = participants }: Conversation.T)
    =

    let date (m: Conversation.Message) =
        m.Timestamp.Date.ToShortDateString()

    let countsByDate (m: seq<Conversation.Message>) =
        m
        |> Seq.groupBy date
        |> Seq.map (fun (g, ms) -> g, ms |> Seq.length)

    let messagesBySender =
        messages
        |> Seq.groupBy (fun m -> m.SenderName)

    let data =
        messagesBySender
        |> Seq.map (fun (g, ms) -> g, countsByDate ms)
    
    let bars =
        data
        |> Seq.map
            (fun (g, m) ->
                let x, y = m |> Array.ofSeq |> Array.unzip
                Bar(x = x, y = y, name = g))

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
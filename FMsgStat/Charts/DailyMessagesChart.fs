module DailyMessagesChart

open System

open XPlot.Plotly

type MessagesGroupKey =
    { Year: int
      Month: int
      Day: int
      Sender: string }

let dailyMessagesChart
    ({ Messages = messages
       Participants = participants }: Conversation.T)
    =

    let messagesByDateAndSender =
        messages
        |> Array.groupBy
            (fun f ->
                let dt =
                    DateTimeOffset.FromUnixTimeMilliseconds f.TimestampMs

                { Year = dt.Year
                  Month = dt.Month
                  Day = dt.Day
                  Sender = f.SenderName })

    let messageGroupsBySender =
        messagesByDateAndSender
        |> Array.groupBy (fun (g, _) -> g.Sender)

    let messageGroupsToAxes (groups: (MessagesGroupKey * MessageFile.Message []) []) =
        groups
        |> Array.map
            (fun (g, m) ->
                {| X =
                       DateTime(g.Year, g.Month, g.Day)
                           .ToShortDateString()
                   Y = m.Length |})

    let traces =
        messageGroupsBySender
        |> Array.map
            (fun (g, m) ->
                let axes = messageGroupsToAxes m

                let x, y =
                    axes
                    |> Array.map (fun el -> el.X, el.Y)
                    |> Array.unzip

                Bar(x = x, y = y, name = g))

    let title = participants |> String.concat ", "

    let stackedLayout = Layout(barmode = "stack")

    let chart =
        traces
        |> Chart.Plot
        |> Chart.WithLayout stackedLayout
        |> Chart.WithHeight 800
        |> Chart.WithWidth 1200
        |> Chart.WithTitle title

    chart
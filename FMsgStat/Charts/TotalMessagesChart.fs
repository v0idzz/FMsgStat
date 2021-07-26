module TotalMessagesChart

open XPlot.Plotly

let totalMessagesChart
    ({ Messages = messages
       Participants = participants }: Conversation.T)
    =
    let messagesBySender =
        messages |> Array.groupBy (fun m -> m.SenderName)

    let traces =
        messagesBySender
        |> Array.map (fun (g, m) -> Bar(x = [|"Total"|], y = [|m.Length|], name = g))

    let title = participants |> String.concat ", "

    let groupedLayout = Layout(barmode = "group")

    traces
    |> Chart.Plot
    |> Chart.WithLayout groupedLayout
    |> Chart.WithHeight 800
    |> Chart.WithWidth 1200
    |> Chart.WithTitle title
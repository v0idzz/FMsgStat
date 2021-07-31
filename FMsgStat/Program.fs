open System.IO

let chartsGenerators =
    [ "Daily", DailyMessagesChart.dailyMessagesChart
      "Total", TotalMessagesChart.totalMessagesChart
      "Weekly", AverageWeeklyMessagesChart.averageWeeklyMessagesChart ]

[<EntryPoint>]
let main argv =
    let messageDirectories = Directory.GetDirectories("messages")

    let conversations =
        messageDirectories
        |> Array.map
            (fun d ->
                let messageFiles =
                    Directory.GetFiles(d, "message*.json")
                    |> Array.map (fun f -> f |> MessageFile.readFromFile)

                (d, Conversation.fromMessageFiles messageFiles))

    let conversationsCharts =
        conversations
        |> Array.map
            (fun c ->
                {| Charts =
                       (chartsGenerators
                        |> List.map (fun (name, gen) -> (name, gen (snd c))))
                   DirectoryAndConversation = c |})

    conversationsCharts
    |> Array.iter
        (fun c ->
            c.Charts
            |> List.iter
                (fun (name, chart) ->
                    let html = chart.GetHtml()

                    let directory =
                        Path.Combine(fst c.DirectoryAndConversation, "charts")

                    if not (Directory.Exists directory) then
                        Directory.CreateDirectory(directory) |> ignore

                    File.WriteAllText(Path.Combine(directory, $"{name}.html"), html)))


    0

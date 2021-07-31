module Conversation

open System

type Message =
    { SenderName: string
      Timestamp: DateTimeOffset
      Content: string }

type T =
    { Participants: string []
      Messages: Message [] }

let convertTimestamps (m: MessageFile.RawMessage) =
    { SenderName = m.SenderName
      Timestamp =
          m.TimestampMs
          |> DateTimeOffset.FromUnixTimeMilliseconds
      Content = m.Content }

let fromMessageFiles (messageFiles: MessageFile.T []) =
    { Participants =
          messageFiles.[0].Participants
          |> Array.map (fun p -> p.Name)
      Messages =
          messageFiles
          |> Array.collect (fun m -> m.Messages)
          |> Array.map convertTimestamps }

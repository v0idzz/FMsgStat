module Conversation

type T =
    { Participants: string []
      Messages: MessageFile.Message [] }

let fromMessageFiles (messageFiles: MessageFile.T []) =
    { Participants =
          messageFiles.[0].Participants
          |> Array.map (fun p -> p.Name)
      Messages =
          messageFiles
          |> Array.collect (fun m -> m.Messages) }

module MessageFile

open System.Globalization
open System.IO
open System.Text.Json
open System.Text.RegularExpressions

type Participant = { Name: string }

type Message =
    { SenderName: string
      TimestampMs: int64
      Content: string }
    
type Contents = {
    Participants: Participant []
    Messages: Message []
}

type T =
    { Participants: Participant []
      Messages: Message [] }

type SnakeCaseNamingPolicy() =
    inherit JsonNamingPolicy()

    override x.ConvertName(name) =
        let ti = CultureInfo("en-US", false).TextInfo

        Regex("(?<=\w)([A-Z])").Replace(name, "_$1")
        |> ti.ToLower

let readFromFile fileName =
    let namingPolicy =
        JsonSerializerOptions(PropertyNamingPolicy = SnakeCaseNamingPolicy())

    let content = File.ReadAllText fileName
    JsonSerializer.Deserialize<T>(content, namingPolicy)

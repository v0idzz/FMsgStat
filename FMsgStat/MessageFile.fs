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

type Contents =
    { Participants: Participant []
      Messages: Message [] }

type T =
    { Participants: Participant []
      Messages: Message [] }

let private snakeCaseNamingPolicyOptions =
    JsonSerializerOptions(
        PropertyNamingPolicy =
            (let ti =
                CultureInfo.GetCultureInfo("en-US").TextInfo

             let re = Regex("(?<=\w)([A-Z])")

             { new JsonNamingPolicy() with
                 member _.ConvertName(x) = re.Replace(x, "_$1") |> ti.ToLower })
    )

let readFromFile fileName =
    let content = File.ReadAllText fileName
    JsonSerializer.Deserialize<T>(content, snakeCaseNamingPolicyOptions)

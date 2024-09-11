## RE# - An unconventional high-performance regex engine supporting intersection and complement

web application and examples: [https://iev.ee/resharp/](https://iev.ee/resharp/)

## Important syntax

- `_` - any character (same as [\s\S] or [\w\W])
- `~` - complement, ex. "does not contain 1" => `~(_*1_*)`
- `&` - intersection, ex. "contains cat and dog and 5-15 chars long" => `_*cat_*&_*dog_*&_{5,15}`

> Note:
> The public API is still subject to change without backwards compatibility. 
> Beta testers are very welcome!

## Basic usage

```fsharp
#r "nuget: resharp, 0.0.18"
let matches =
    Resharp.Regex("hello.*world").Matches("hello world!")
```

## High performance usage on slices 
> note: the byte stream api is not finished so file must fit into memory
```fsharp
#r "nuget: resharp, 0.0.18"
open System
open System.IO
// building the engine is the most expensive part, reuse this
let regex = Resharp.Regex("abc", Resharp.ResharpOptions.HighThroughputDefaults)
let matchOnFile(filePath:string) =  
    // part 1. read the file without allocating
    // if you dont care about this part and your file is 
    // below string max size just do `File.ReadAllText("")`
    let info = FileInfo(filePath)
    let rentedBytes = Buffers.ArrayPool<byte>.Shared.Rent(int info.Length)
    let byteSpan = rentedBytes.AsSpan().Slice(0,int info.Length)
    use stream = File.OpenRead(filePath)
    stream.ReadExactly(byteSpan)
    let decoder = Text.Encoding.Default.GetDecoder()
    let charCount = decoder.GetCharCount(byteSpan, false)
    let rentedChars = Buffers.ArrayPool<char>.Shared.Rent(int charCount)
    let charSpan = rentedChars.AsSpan(0,charCount)
    let _ = decoder.GetChars(byteSpan, charSpan, false)
    // part 2. matching without allocating
    use results = regex.MatchSlices(charSpan)
    for slice in results do 
        // only allocate text if you need to
        let matchText = slice.GetText(charSpan)
        stdout.WriteLine 
            $"{slice.Index} {slice.Length} : {matchText}"
    // part 3. return pooled resources
    Buffers.ArrayPool<byte>.Shared.Return(rentedBytes)
    Buffers.ArrayPool<char>.Shared.Return(rentedChars)

```


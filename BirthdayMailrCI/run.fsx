#r "System.Net.Http"
#r "Newtonsoft.Json"

open System.Net
open System.Net.Http
open Newtonsoft.Json
 
type Named = {
    name: string
}

let Run(req: HttpRequestMessage, log: TraceWriter) =
    async {
        log.Info(sprintf "F# HTTP trigger function processed a request.")

        let people =  [
                            ("Mammy","24 March 1981")
                            ("Grandad","15 September 1942")
                            ("Daddy","14 November 1978")
                            ("Charlie","27 February 2015")
                            ("Eamonn","4 October 2012")
                            ("Granny","24 June 1948")
                            ("Auntie Sinead","29 August 1976")
                            ("Auntie Lorraine","13 April 1983")
                            ("Auntie Caitriona","December 17 1979")
                            ("Auntie Orlatha","27 January 1985")
                            ("Baby Ryan","2 June 2016")
                            ("Eabha","9 April 2008")
                            ("Nana","24 October 1950")
                            ("Joebadabo","17 March 1950")
                            ("Auntie Grainne","25 September 1970")
                            ("Hunkle Eamonn","6 October 1971")
                            ("Auntie Aisling","1 November 1972")
                            ("Auntie Doireann","9 July 1975")
                            ("Auntie Meabh","10 February 1977")
                            ("Janine","14 April 1969")
                            ("Paraic","1 February 1979")
                            ("Kevin","23 February 1978")
                        ]
                        
                        
        let calcyears x =
            let dtx = System.DateTime.Parse (x)
            let days = (DateTime.Now - dtx).TotalDays
            let years = days / 365.0
            int years
                
        let calcmonths x =
            let dtx = System.DateTime.Parse (x)
            let dtm = new DateTime (DateTime.Now.Year, dtx.Month, dtx.Day)
            let days = (DateTime.Now - dtm).TotalDays
            let absdays = match days with 
            | i when i < 0.0 -> days + 365.0
            | _ -> days
            let months = absdays / 30.0
            int months
        //    days 

        let calcdaystonextbirthday x =
            let dtx = System.DateTime.Parse (x)
            let dtm = new DateTime (DateTime.Now.Year, dtx.Month, dtx.Day)
            let days = (dtm - DateTime.Now).TotalDays
            let absdays = match days with 
                | i when i < 0.0 -> days + 365.0
                | _ -> days
            int absdays

        let sb = new System.Text.StringBuilder()

        let ppldob = people |> List.sortBy (fun person -> calcdaystonextbirthday (snd person))
        ppldob |> List.iter (fun person ->  let newp = ((fst person), ((calcdaystonextbirthday (snd person) / 7)), (calcdaystonextbirthday (snd person)))
                                            match newp with 
                                            | n,0,0 -> sb.Append (sprintf "%s's birthday is TODAY\n" n)|> ignore
                                            | n,0,1 -> sb.Append (sprintf "%s's birthday is Tomorrow\n" n)|> ignore
                                            | n,0,d -> sb.Append (sprintf "%s's birthday is this %O\n" n (System.DateTime.Now.AddDays (float d)).DayOfWeek)|> ignore
                                            | n,1,d -> sb.Append (sprintf "%s's birthday is next %O\n" n (System.DateTime.Now.AddDays (float d)).DayOfWeek)|> ignore
                                            | n,w,d when (d>1 && d<6)-> sb.Append (sprintf "%s's birthday is in %O weeks\n" n (w-1))|> ignore
                                            //| n,_,_-> Console.Write (sprintf "%s's birthday is ages away\n" n)
                                            | _,_,_-> ()
                                        )
                                        
        sb.Append (sprintf "\n")|> ignore
                                        
        // let pplage = people |> List.sortBy (fun person -> System.DateTime.Parse (snd person))
        // pplage |> List.iter (fun person ->  let newp = ((fst person), (calcyears (snd person)), (calcmonths (snd person)))
        //                                     match newp with 
        //                                     | n,0,m -> sb.Append (sprintf "%s is just %d months old\n" n m)|> ignore
        //                                     | n,y,0 -> sb.Append (sprintf "%s is %d years old\n" n y)|> ignore
        //                                     | n,y,1 -> sb.Append (sprintf "%s is %d years and 1 month old\n" n y)|> ignore
        //                                     | n,y,6 -> sb.Append (sprintf "%s is %d and a half\n" n y)|> ignore
        //                                     | n,y,m -> sb.Append (sprintf "%s is %d years and %d months old\n" n y m)|> ignore
        //                                 )
            

        
        // let oldest = pplage
        //                 |> List.head
        
        // let youngest = pplage
        //                 |> List.rev
        //                 |> List.head
        
        // sb.Append (sprintf "\n") |> ignore
        // sb.Append (sprintf "%s is the oldest\n" (fst oldest))|> ignore
        // sb.Append (sprintf "%s is the youngest\n" (fst youngest))|> ignore

        log.Info (sb.ToString())
        return req.CreateResponse(HttpStatusCode.OK, sb.ToString());
        // // Set name to query string
        // let name =
        //     req.GetQueryNameValuePairs()
        //     |> Seq.tryFind (fun q -> q.Key = "name")

        // match name with
        // | Some x ->
        //     return req.CreateResponse(HttpStatusCode.OK, "Hello " + x.Value);
        // | None ->
        //     let! data = req.Content.ReadAsStringAsync() |> Async.AwaitTask

        //     if not (String.IsNullOrEmpty(data)) then
        //         let named = JsonConvert.DeserializeObject<Named>(data)
        //         return req.CreateResponse(HttpStatusCode.OK, "Hello " + named.name);
        //     else
        //         return req.CreateResponse(HttpStatusCode.BadRequest, "Specify a Name value");
    } |> Async.RunSynchronously

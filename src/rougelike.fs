module rougeGame


//11g0
//starts by defining the type Color
type Color = System.ConsoleColor
// type emptyArray (c : )
/// <summary> This class initiates a canvas with the given arguments being the width (cols) and height (rows) </summary>
/// <remarks> The canvas is represented using a 2D array </remarks>
/// <param name = "rows"> The height of the canvas </param>
/// <param name = "dols"> The width of the canvas </param>
type Canvas(rows : int, cols : int) = 
    let arrayToShow = Array2D.init rows cols (fun i j -> (' ', Color.White, Color.Black))
    member this.rows = rows
    member this.cols = cols
    member val array = arrayToShow with get, set

    /// <summary> The method sets the properties of a specific pixel (index in the 2D array) on the canvas </summary>
    /// <param name = "x"> The x-coordinat of the canvas </param>
    /// <param name = "y"> The y-coordinat of the canvas </param>
    /// <param name = "c"> The character to be shown on specific pixel </param>
    /// <param name = "fg"> The color of the character </param>
    /// <param name = "bg"> The color of the background </param>
    /// <returns> A unit </returns>
    member this.Set ((x : int, y : int), (c : char, fg : Color,bg : Color)) = 
        this.array.[y, x] <- (c, fg, bg)

    /// <summary> Shows the canvas in the commandprompt (By printing the 2D-array) </summary>
    /// <returns> A unit </returns>
    member this.Show () : unit = 
        System.Console.Clear()
        for i in 0..rows - 1 do
            printfn ""
            for j in 0..cols - 1 do
                let (c, fg, bg) = this.array.[i, j]
                System.Console.BackgroundColor <- bg
                System.Console.ForegroundColor <- fg
                printf "%c" c 
                System.Console.ResetColor()
        printfn ""
    /// <summary> The winning message, when succesfully exiting the dungeon </summary>
    /// <returns> A unit </returns>
    member this.Won () : unit = 
        System.Console.Clear()
        printfn "Tillykke! Du fandt ud af grotten!"

    /// <summary> The loosing message </summary>
    /// <returns> A unit </returns>
    member this.Dead () : unit = 
        System.Console.Clear()
        printfn "Du døede, spillet er slut"

//11g1
/// <summary> This class is an "abstract class" since it just help defines its children </summary>
/// <param name = "pos"> The x and y coordinat of the initated entity </param>
/// <param name = "info"> The character to be shown on specific pixel, the color of the character and the color of the background </param>
type Entity(pos : (int * int), info : (char * Color * Color)) = 
    member val pos = pos with get, set
    member val info = info with get, set
    /// <summary> This method renders the Entity on the canvas </summary>
    /// <param name = "canvas"> The canvas which the entity should be rendered on</param>
    /// <returns> A unit </returns>
    abstract member RenderOn : Canvas -> unit
    default this.RenderOn canvas = 
        canvas.Set ((this.pos), (this.info))
    
    
/// <summary> This class initiates the a player </summary>
/// <param name = "pos"> The start position of the player </param> 
type Player (pos : (int * int)) = 
    inherit Entity(pos, ('@', Color.White, Color.DarkMagenta))
    member val sanity = 400 with get, set
    member val HitPoints = 10 with get, set
    member val IsDead = false with get, set
    member val inventory = [] with get, set
    member val won = false with get, set

    /// <summary> This method damages the player a certain amount </summary>
    /// <param name = "canvas"> The amount of damage the player should take </param>
    /// <returns> A unit </returns>
    member this.Damage(dmg : int) = 
        if this.HitPoints - dmg <= 0 then
            this.IsDead <- true
        else 
            this.HitPoints <- this.HitPoints - dmg
    /// <summary> This method heals the player a certain amount </summary>
    /// <remarks> The player can at maximum have 20 HP </remarks>
    /// <param name = "canvas"> The amount of HP the player should be healed </param>
    /// <returns> A unit </returns>
    member this.Heal (h : int) = 
        if this.HitPoints + h >= 20 then
            this.HitPoints <- 20
        else 
            this.HitPoints <- this.HitPoints + h
    /// <summary> This method makes the player loos 1 sanity </summary>
    /// <remarks> Each time the player moves he/she loses 1 sanity, if the player reaches 0 sanity the player will go insane and is destined to death. </remarks>
    /// <returns> A unit </returns>
    member this.removeSanity() = 
        if this.sanity = 1 then
            this.IsDead <- true
        else
            this.sanity <- this.sanity - 1 
    /// <summary> This moves the player to a given amount </summary>
    /// <param name = "x"> The amount the player should move on the x-axis (both negative and positive) </param>
    /// <param name = "y"> The amount the player should move on the y-axis (both negative and positive)  </param>
    /// <returns> A unit </returns>
    member this.MoveTo ((x : int, y : int)) = 
        this.removeSanity()
        this.pos <- (fst this.pos + x, snd this.pos + y)
        
/// <summary> This class inherites the Entity class and therefore takes the same argements </summary>
type Item(pos : (int * int), info : (char * Color * Color)) = 
   inherit Entity(pos, info)
   /// <summary> This method takes care of the interraction between initated items and entity of the player class </summary>
   /// <param name = "player"> The player of which the item should interract with </param>
   /// <returns> A unit </returns>
   abstract member InteractWith : Player -> unit
   default this.InteractWith player = ()
   abstract member FullyOccupy : bool with get, set
   default val FullyOccupy = true with get, set
   abstract member RenderThis : bool with get, set
   default val RenderThis = true with get, set


//Forskellige items
/// <summary> The class Wall which inherites the Item class. </summary>
/// <param name = "pos"> The position at which the item should be placed on the canvas </param>
type Wall (pos : (int * int)) = 
    inherit Item(pos, (' ', Color.White, Color.DarkGray))

/// <summary> The class Fire which inherites the Item class. </summary>
/// <param name = "pos"> The position at which the item should be placed on the canvas </param>
type Fire  (pos : (int * int)) =
    inherit Item(pos, ('F', Color.Red, Color.Red))
    member val counter = 0 with get, set
    override this.FullyOccupy = false
    override this.RenderOn(canvas : Canvas) =
        if this.RenderThis = false then
            ()
        else    
            canvas.Set ((this.pos), (this.info))
    override this.InteractWith (player : Player) = 
          if this.counter < 4 && this.RenderThis = true then
              this.counter <- this.counter + 1
              player.Damage(1)
          elif this.counter = 4 && this.RenderThis = true then
              player.Damage(1)
              this.RenderThis <- false
          else
              ()
/// <summary> The class Water which inherites the Item class. </summary>
/// <param name = "pos"> The position at which the item should be placed on the canvas </param>
type Water  (pos : (int * int)) =
    inherit Item(pos, ('W', Color.White, Color.Blue))
    member val counter = 0 with get, set
    override this.FullyOccupy = false
    override this.RenderOn(canvas : Canvas) =
        if this.RenderThis = false then
            ()
        else
            canvas.Set ((this.pos), (this.info))
    override this.InteractWith (player : Player) = 
        if this.counter = 0 then
            player.Heal (2)
            this.counter <- 1
            this.RenderThis <- false
        else
            ()
/// <summary> The class FleshEatingFleshEatingPlant which inherites the Item class. </summary>
/// <param name = "pos"> The position at which the item should be placed on the canvas </param>
type FleshEatingPlant (pos : (int * int)) =
    inherit Item(pos, ('P', Color.White, Color.Green))
    override this.InteractWith (player : Player) = 
                player.Damage(5)

/// <summary> The class Exit which inherites the Item class. </summary>
/// <param name = "pos"> The position at which the item should be placed on the canvas </param>
type Exit (pos : (int * int)) =
    inherit Item(pos, ('E', Color.Blue, Color.Yellow))
    override this.InteractWith (player : Player) = 
        if player.HitPoints >= 5 then
            player.won <- true
        elif player.HitPoints < 5 then
            printfn "You dont have enough HP to get the door open! - Press any key to continue!"
            System.Console.ReadKey() |> ignore
        else 
            ()
/// <summary> The class CodeDoor which inherites the Item class.  </summary>
/// <param name = "pos"> The position at which the item should be placed on the canvas </param>
/// <param name = "password"> The password needed to open the door </param>
type CodeDoor (pos : (int * int), password : string) = 
    inherit Item(pos, ('D', Color.White, Color.Black))
    override this.RenderOn(canvas : Canvas) =
        if this.RenderThis = false then
            ()
        else
            canvas.Set (this.pos, this.info)
    override this.InteractWith (player : Player) = 
        if this.RenderThis = true then
            printf "Password: "
            match System.Console.ReadLine() with
            | code when code = password ->
                this.FullyOccupy <- false
                this.RenderThis <- false
            | code when code = "" -> ()
            | _ -> 
                printf "The password was wrong, try again!"
                player.Damage(2)
                System.Console.ReadKey() |> ignore       
        else
            ()

/// <summary> The class hint which inherites the Item class.  </summary>
/// <param name = "pos"> The position at which the item should be placed on the canvas </param>
/// <param name = "hint"> A hint of information, maybe as to what the code of a certain door is </param>
type Hint (pos : (int * int), hint : string) = 
    inherit Item(pos, ('H', Color.Black, Color.White))
    override this.FullyOccupy = false
    override this.RenderOn(canvas : Canvas) =
        if this.RenderThis = false then
            ()
        else
            canvas.Set ((this.pos), (this.info))
    override this.InteractWith (player : Player) = 
        if this.RenderThis = true then
            this.RenderThis <- false
            player.inventory <- hint :: player.inventory
        else 
            ()  

type Teleport (pos : (int * int), destination : (int * int)) = 
    inherit Item(pos, ('T', Color.Blue, Color.Magenta))
    override this.InteractWith (player : Player) = 
        player.pos <- (destination) 



/// <summary> This class a world for which the game takes place </summary>
type World(player : Player, size : (int * int)) = 
    let canvas = Canvas (fst size, snd size)
    let mutable listOfItems : Item list = []
    member val theEnd = false with get, set
    /// <summary> This method Adds items from the Item class to the overall list of items, which shall be 
    /// rendered on the canvas </summary>
    /// <param name = "item"> A initiatien of the Item class </param>
    /// <returns> A unit </returns>
    member this.AddItem (item : Item) : unit =
        listOfItems <- item :: listOfItems
    /// <summary> This method renders all the items of the given list on hte given canvas </summary>
    /// <param name = "canvas"> A canvas of the class Canvas </param>
    /// <param name = "item"> A initiatien of the Item class </param>
    /// <returns> A unit </returns>
    member this.RenderItems (canvas : Canvas, lst : Item list) : unit = 
        for i in lst do
            if sqrt(((float (fst (i.pos)) -  float (fst player.pos)) ** 2.0) + ((float (snd i.pos) - float (snd player.pos)) ** 2.0)) >= 50.0 then
                ()
            else
                i.RenderOn(canvas)
    /// <summary> This method checks if any items of the item list, will interract (have same position) as player is about to move to </summary>
    /// <param name = "newPos"> The amount the players new position is different from the on prior to Moving </param>
    /// <returns> A unit </returns>
    member this.itemInteraction (newPos : (int * int)) =
        match listOfItems |> List.tryFind (fun item -> item.pos = (fst player.pos + fst newPos, snd player.pos + snd newPos)) with
        |Some a -> a.InteractWith(player)
                   if a.FullyOccupy = true then 
                       ()
                   else
                        player.MoveTo(fst newPos, snd newPos)
        |None -> player.MoveTo(fst newPos, snd newPos)
    /// <summary> This method checks for the players movement, by checking which arrowkey is being pressed </summary>
    /// <remarks> Every other key than the 4 arrowkeys are ignored </remarks>
    /// <returns> A unit </returns>
    member this.Moving() =
        let rec move () = 
            let key = System.Console.ReadKey().Key
            match (key) with
            | k when k = System.ConsoleKey.UpArrow -> 
                if snd player.pos - 1 < 0 then
                    move()
                else 
                    this.itemInteraction (0, -1)
            | k when k = System.ConsoleKey.DownArrow -> 
                if snd player.pos + 1 > fst size then
                    move()
                else 
                    this.itemInteraction (0, 1)
            | k when k = System.ConsoleKey.LeftArrow -> 
                if fst player.pos - 1 < 0 then
                    move()
                else
                    this.itemInteraction (-1, 0)
            | k when k = System.ConsoleKey.RightArrow -> 
                if fst player.pos + 1 > snd size then
                    move()
                else
                    this.itemInteraction (1, 0)
            | k -> k |> ignore; move()
        move()
    /// <summary> This method creates a boundery all along the edges of the canvas consisting of the initiations of the Wall class </summary>
    /// <param name = "canvas"> An initiation of the canvas class  </param>
    /// <returns> A unit </returns>
    member this.createBounderies(canvas : Canvas) = 
        let mutable listWalls : (int * int) list = []
        for i in 0..canvas.cols - 1 do
            listWalls <- (i,0) :: listWalls
        for i in 0..canvas.rows - 1 do
            listWalls <- (0, i) :: listWalls
            listWalls <- (canvas.cols - 1, i) :: listWalls
        for i in 0..canvas.cols - 1 do
            listWalls <- (i, canvas.rows - 1) :: listWalls
        List.map (fun x -> Wall(x) :> Item) listWalls
    
    /// <summary> This method initiates the game </summary>
    /// <returns> A unit </returns>
    member this.Play () = 
        //Inititerer start positionerne for spilleren og items
        let bounderies = this.createBounderies (canvas)
        listOfItems <- bounderies @ listOfItems
        player.RenderOn(canvas)
        this.RenderItems(canvas, listOfItems)
        canvas.Show()
        System.Console.CursorVisible <- false 
        while this.theEnd = false do
            if  player.IsDead then
                this.theEnd <- true
                canvas.Dead()
            elif player.won then
                this.theEnd <- true
                canvas.Won()
            else
                let canvas1 = Canvas(fst size, snd size)  
                this.Moving()
                this.RenderItems(canvas1, listOfItems)
                player.RenderOn(canvas1)
                canvas1.Show()
                printfn "HP: %A / 20" player.HitPoints
                printfn "Sanity: %A/400" player.sanity
                printfn "Pos: %A" player.pos
                printfn "Inventory: %A" player.inventory



module rougeGame
open rougeGame


let FleshEatingPlants = [(39,1); (39,2); (40,4); (39,4); (38,4); (37,4); (37,3); (37,2);(35,1);(35,2);(35,3);(35,4);(40,6);(39,6);(38,6);(37,6);(36,6);(35,6);(34,6);(33,6);(33,5);(33,4);(33,3);(33,2)]
let Walls = [(36,12); (35,12); (35,13); (35,14); (35,15); (38,12); (39,12); (40,12); (2,13); (2, 12); (2,11); (2, 10); (2,9); (2,8); (2,7); (2,6); (2,5); (2,4); (3,13);
 (4,13); (4,14); (4,15); (15,5); (15,6); (15,7); (15,8); (16,8); (17,8); (18,8); (19,8); (19,7); (19,6); (19,5); (18,5); (17,5); (16,5); (15,5)]
let Fires = [(1,13); (1, 12); (1,11); (1, 10); (1,9); (1, 8); (1,7); (1,6); (1,5); (1,4); (20,9); (21,10);(22,11); (23,12); (24,13); (25,14);(26,15)]
let Waters = [(1,14); (2, 14); (3,14); (3, 15);(18,6); (17,6);(16,7)]
///<summary> These function are helper functions, which helps create a list of items that should be added to the world. Each function
/// takes a list of coordinates and each one respectively creates and initiation of either the class FleshEatingPlants, Wall, Fire or Water.
/// Each funtion makes use of the function List.map </summary>
/// <param name = "lst"> A list of coordinates </param>
/// <returns> An Item list </returns>
let createFleshEatingPlants (lst : (int * int) list) =
    List.map (fun x -> FleshEatingPlant(x) :> Item) lst
let createWalls (lst : (int * int) list) = 
    List.map (fun x -> Wall(x) :> Item) lst
let createFire (lst : (int * int) list) =
    List.map (fun x -> Fire(x) :> Item) lst
let createWater (lst : (int * int) list) =
    List.map (fun x -> Water(x) :> Item) lst

//Initiates the world of which the games is to be played
let player1 = Player(1,1)
let world = World(player1, (17,42))
//Creates a mutable list where all the items which shall be added to the world, can be put into for storage purposes.
let mutable listOfItems = []
//Adds a CodeDoor with the code 356
listOfItems <- (CodeDoor ((37,12), "356") :> Item) :: listOfItems
//Adds all the flesh eating plants.
listOfItems <- createFleshEatingPlants FleshEatingPlants @ listOfItems
//Adds all the walls
listOfItems <- createWalls Walls @ listOfItems
//Adds all the walls water
listOfItems <- createWater Waters @ listOfItems
//Adds all the Fire
listOfItems <- createFire Fires @ listOfItems
//Adds the 4 hints which shall help the player crack the code of the codedoor
listOfItems <- Hint((40,1), "Hint 1/4: Koden har 3 cifre!") :> Item :: listOfItems
listOfItems <- Hint((1,15), "Hint 2/4: Det 3. cifre i koden er vores (Johannes, Johannes og Mattias) gruppenummer i PoP!") :> Item :: listOfItems
listOfItems <- Hint((17,7), "Hint 3/4: Det 2. cifre er et primtal, der mindre end det 3. cifre men større end det 1. cifre!") :> Item :: listOfItems
listOfItems <- Hint((14,15), "Hint 4/4: Det 1. cifre er antallet af kurser Dat/øk'er har i blok 1 + blok 2!") :> Item :: listOfItems
//Adds 3 teleportes who helps the player move around the cave.
listOfItems <- Teleport((34, 15), (18,7)) :> Item :: listOfItems
listOfItems <- Teleport((40, 5), (2,15)) :> Item :: listOfItems
listOfItems <- Teleport((16, 6), (39, 10)) :> Item :: listOfItems
//Adds the exit of the cave
listOfItems <- Exit(38,14) :> Item :: listOfItems
//Adds alle the items of the list (listOfItems) to the world with the use of the method world.Additem and a for-loop
for i in listOfItems do
    world.AddItem (i)
System.Console.CursorVisible <- false 
//Starts the game
world.Play()
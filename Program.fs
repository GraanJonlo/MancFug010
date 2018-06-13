open System

// Domain

type GameState = {
    score:int
    message:string
}

type MoveResult =
    | PlayerQuit
    | PlayerToTakeTurn

type NewGame = 
    GameState * MoveResult

type Command =
    Quit

type PlayerMove =
    GameState -> Command ->
        GameState * MoveResult

type Api = {
    newGame:NewGame
    playerMove:PlayerMove
}

// Implementation

let newGame = { score=0; message="Welcome to Foo World!\n" }, PlayerToTakeTurn

let playerMove state command =
    match command with
    | Quit -> { state with message="I guess it was all too much" }, PlayerQuit

let api = {
    newGame = newGame 
    playerMove = playerMove
}

// UI

let processInput state makeMove =
    let inputStr = Console.ReadLine()
    makeMove state Quit

let displayState state =
    printfn "%s" state.message
    printfn "Current score: %d" state.score

let rec gameLoop api (state, moveResult) = 
    printfn "\n------------------------------\n"
    state |> displayState

    match moveResult with
    | PlayerQuit ->
        printfn "Exiting game."
    | PlayerToTakeTurn ->
        printfn "What is your command?"
        let newResult = processInput state api.playerMove
        gameLoop api newResult

let startGame api =
    gameLoop api api.newGame

[<EntryPoint>]
let main _ =
    startGame api
    0

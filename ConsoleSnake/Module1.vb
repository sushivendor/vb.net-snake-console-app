Module Module1
    'Notes when reading this code
    '"Pos" shortened "postision"
    '"Dir" shortened "direction
    'declare objects used
    Dim consolsnake As New Game()                           'new game called consolsnake
    Dim snakeBody As New List(Of Snake)                     'list of snake block objects 
    Dim mouse As New Food()                                 'the snake eats the mouse

    Sub Main()
        'initialise console window height and width and buffer size
        Console.SetWindowSize(50, 25)
        Console.SetBufferSize(52, 27)
        Console.Title = "Console App Snake"

        'variable for user choice
        Dim choice As String = "0"
        'while user doesnt exit, output menu again
        While choice < "2"
            Console.Clear()
            Console.CursorVisible = True
            OutMenu()
            Console.Write("         Enter number to select: ")
            choice = Console.ReadLine()
            'ask for user input
            While Val(choice) < 1 Or Val(choice) >= 3            'loop to ensure input is correct
                Console.Clear()
                OutMenu()
                Console.Write("         Enter number to select: ")
                choice = Console.ReadLine()
            End While
            'select menu
            Select Case choice
                Case "1"    'play game
                    snakeBody.Clear()                           'clear snake list
                    PlayGame()                                  'runs game subroutine
                Case "2"    'exit
                    end
            End Select
        End While
    End Sub
    Sub PlayGame()
        'some variables about current game info
        Dim button As New ConsoleKeyInfo()                  'create consolekeyinfo object for controls
        Dim startTime As Integer = Environment.TickCount    'time when player starts a game relative to elapsed time of the console application
        Dim moveAtTime As Integer = 0
        'intialise some stuff
        Dim sneke As New Snake()
        '   place snake at the middle
        sneke.X = Console.WindowWidth / 2
        sneke.Y = Console.WindowHeight / 2
        snakeBody.Add(sneke)
        '   initialise random
        Randomize()
        GetNewFood()
        consolsnake.Points = 0
        consolsnake.GameOver = False

        'GAME LOOP
        While consolsnake.GameOver = False
            consolsnake.CurrentTime = Environment.TickCount                       'Update time
            'CONTROLS HANDLER
            'if there is key pressed then check what key it is
            If Console.KeyAvailable Then
                button = Console.ReadKey(True)                                      'store key info into button variable
                If button.Key.Equals(ConsoleKey.W) Or button.Key.Equals(ConsoleKey.UpArrow) Then 'if up (w) is pressed
                    If Not (consolsnake.DirX = 0 And consolsnake.DirY = 1) Then
                        consolsnake.DirX = 0                                        'make x axis direction + 0
                        consolsnake.DirY = -1                                       'make y axis direction + 1
                    End If
                End If
                If button.Key.Equals(ConsoleKey.S) Or button.Key.Equals(ConsoleKey.DownArrow) Then 'if down(s) is pressed
                    If Not (consolsnake.DirX = 0 And consolsnake.DirY = -1) Then
                        consolsnake.DirX = 0                                       'make x axis direction + 0
                        consolsnake.DirY = 1                                       'make y axis direction - 1
                    End If
                End If
                If button.Key.Equals(ConsoleKey.A) Or button.Key.Equals(ConsoleKey.LeftArrow) Then 'if left (a) is pressed
                    If Not (consolsnake.DirX = 1 And consolsnake.DirY = 0) Then
                        consolsnake.DirX = -1                                       'make x axis direction - 1
                        consolsnake.DirY = 0                                        'make y axis direction + 0
                    End If
                End If
                If button.Key.Equals(ConsoleKey.D) Or button.Key.Equals(ConsoleKey.RightArrow) Then 'if right(d) is pressed
                    If Not (consolsnake.DirX = -1 And consolsnake.DirY = 0) Then
                        consolsnake.DirX = 1                                        'make x axis direction + 1
                        consolsnake.DirY = 0                                        'make y axis direction + 0
                    End If
                End If
            End If
            'SNAKE MOVEMENT HANDLER
            If consolsnake.CurrentTime > moveAtTime Then
                UpdateSnakePos()
                CheckBodyTouch()
                CheckWallTouch()
                CheckFood()
                moveAtTime = consolsnake.CurrentTime + 200  'snake moves forward every 200 milliseconds
            End If
            'GRAPHICS HANDLER
            If consolsnake.CurrentTime > consolsnake.RefreshAtTime Then     'refresh every 100 milliseconds
                Console.Clear()
                OutputGameInfo(startTime) 'output game info
                OutputSnake()   'output snake body list
                OutputFood()    'ouput food ar its coordinates
                consolsnake.RefreshAtTime = consolsnake.CurrentTime + 100   'set new time to refresh screen
                Console.CursorVisible = False
            End If
        End While
        Console.Clear()
        OutputGameover()
        Console.ReadKey()
    End Sub


    '   POSITION
    Sub UpdateSnakePos()
        'add new snake body with new coordiantes
        Dim head As New Snake()
        'calculate new coordiantes for head
        head.X += consolsnake.DirX + snakeBody(0).X               'new x direction = (old snake head position + direction)
        head.Y += consolsnake.DirY + snakeBody(0).Y               'new y direction = (old snake head position + direction)
        'new coordinate for body is the old coordinates of head
        Dim body As New Snake()
        body.X = snakeBody(0).X
        body.Y = snakeBody(0).Y
        'adding the new coordinates in the right order
        snakeBody.Item(0) = head                            'change the first index in the list into new coordinatess
        snakeBody.Add(body)                                 'add the body to the bottom of the list
        'remove oldest added body to the list which is index 1(index 0 will always be the head)
        snakeBody.RemoveAt(1)
    End Sub
    Sub GetNewFood()
        Randomize()
        'change x and y value of food to random
        mouse.X = Int((Console.WindowWidth - (1) + 1) * Rnd() + 1)
        mouse.Y = Int(((Console.WindowHeight - 4) - (1) + 1) * Rnd() + 1)
        'follows this formula: Int((upperbound - lowerbound + 1) * Rnd + lowerbound)
    End Sub
    '   GAME OVER CHECK
    Sub CheckWallTouch()
        If snakeBody.First.X = 0 Or snakeBody.First.X = Console.WindowWidth Or snakeBody.First.Y = 0 Or snakeBody.First.Y = Console.WindowHeight - 3 Then
            consolsnake.GameOver = True                       'make the game end
            snakeBody.Clear()                           'clear snake
        End If
    End Sub
    Sub CheckBodyTouch()
        If snakeBody.Count() > 1 Then
            For count = 1 To snakeBody.Count() - 1
                If snakeBody(0).X = snakeBody(count).X And snakeBody(0).Y = snakeBody(count).Y Then
                    consolsnake.GameOver = True
                End If
            Next
        End If
    End Sub
    '   FOOD CHECK
    Sub CheckFood()
        'only check for food if its not game over
        If consolsnake.GameOver = False Then
            'if foodposition and snake position is the same
            If snakeBody(0).X = mouse.X And snakeBody(0).Y = mouse.Y Then               'if the head has the same coordinates
                GetNewFood()                                                            'change coordinates for food
                consolsnake.Points = consolsnake.Points + 1                                         'increment points
                'add new snake body to the list with x and y positions = old position of the head
                Dim body As New Snake()
                body.X = snakeBody.Last.X
                body.Y = snakeBody.Last.Y
                snakeBody.Add(body)

            End If


        End If
    End Sub
    '   SUB ROUTINES FOR OUTPUT (GRAPHICS)
    Sub OutMenu()
        'output for main menu
        Console.WriteLine("--------------------MAIN MENU---------------------")
        Console.WriteLine("")
        Console.WriteLine("        1.  Start")
        Console.WriteLine("        2.  Exit")
        Console.WriteLine("")
        Console.WriteLine("")
        'output highscores here
    End Sub
    Sub OutputSnake()
        For count = 0 To snakeBody.Count - 1
            Console.SetCursorPosition(snakeBody(count).X, snakeBody(count).Y)   'sets position to the coordinates
            Console.Write(snakeBody(count).Symbol)                              'draws the snake body
        Next
    End Sub
    Sub OutputFood()
        'set pixel indicated by coordinate to food
        Console.SetCursorPosition(mouse.X, mouse.Y)     'coordinates of food
        Console.Write(mouse.Symbol)                     'draw symbol of food onto position
    End Sub
    Sub OutputGameInfo(ByVal startTime As Integer)
        Console.SetCursorPosition(0, Console.WindowHeight - 3)
        'boundary
        'Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■")
        Console.WriteLine("▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀")
        'current game information
        Console.WriteLine(" TIME: " & Int((Environment.TickCount - startTime) / 1000) & vbTab & "CONTROLS: WASD or arrow keys")
        Console.WriteLine(" SCORE: " & snakeBody.Count - 1)             'points is amount of snake body - 1 (you start with 1 snake body)

    End Sub
    Sub OutputGameover()
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("           ______________________")
        Console.WriteLine("                 GAME OVER")
        Console.WriteLine("           ______________________")
        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("          SCORE:  " & consolsnake.Points)
        Console.WriteLine()
        Console.WriteLine("          PRESS ANY KEY TO CONTINUE")
    End Sub
    'OBJECTS
    Public Class Snake 'object for snake body
        'variables for property
        Private _posX As Integer = 0                    'x position of snake
        Private _posY As Integer = 0                    'y position of snake
        Private _symbol As Char = "O"                    'Symbol to represent snake

        'Properties
        'X position
        Public Property X() As Integer
            Get
                Return _posX
            End Get
            Set(value As Integer)
                _posX = value
            End Set
        End Property

        'Y position
        Public Property Y() As Integer
            Get
                Return _posY
            End Get
            Set(value As Integer)
                _posY = value
            End Set
        End Property

        'Snake character
        Public Property Symbol() As Char
            Get
                Return _symbol
            End Get
            Set(value As Char)
                _symbol = value
            End Set
        End Property

    End Class

    Public Class Food
        'variables for property
        Private _posX As Integer = 0                    'x position of snake
        Private _posY As Integer = 0                    'y position of snake
        Private _symbol As Char = "■"                   'Symbol to represent food

        'Properties
        'X position
        Public Property X() As Integer
            Get
                Return _posX
            End Get
            Set(value As Integer)
                _posX = value
            End Set
        End Property

        'Y position
        Public Property Y() As Integer
            Get
                Return _posY
            End Get
            Set(value As Integer)
                _posY = value
            End Set
        End Property

        'Snake character
        Public Property Symbol() As Char
            Get
                Return _symbol
            End Get
            Set(value As Char)
                _symbol = value
            End Set
        End Property

    End Class

    Public Class Game      'game object
        'object variables for property
        Private _refreshAtTime As Integer = 0                   'calculated refresh time
        Private _currTime As Integer = 0                        'current time
        Private _dirX As Integer = 0                            'X axis increment  
        Private _dirY As Integer = 0                            'Y axis increment 
        Private _points As Integer = 0                          'points of user/ number of food eaten
        Private _gameOver As Boolean = False                    'Game over flag
        'Properties

        'Refresh time
        'time when the game should refresh during run time
        Public Property RefreshAtTime() As Integer
            Get
                Return _refreshAtTime
            End Get
            Set(value As Integer)
                _refreshAtTime = value
            End Set
        End Property
        'Current time
        'record time during run time
        Public Property CurrentTime() As Integer
            Get
                Return _currTime
            End Get
            Set(value As Integer)
                _currTime = value
            End Set
        End Property
        'Direction on the x axis
        Public Property DirX() As Integer
            Get
                Return _dirX
            End Get
            Set(value As Integer)
                _dirX = value
            End Set
        End Property
        'Direction on the y axis
        Public Property DirY() As Integer
            Get
                Return _dirY
            End Get
            Set(value As Integer)
                _dirY = value
            End Set
        End Property
        'User Points
        Public Property Points() As Integer
            Get
                Return _points
            End Get
            Set(value As Integer)
                _points = value
            End Set
        End Property

        'Game over flag
        Public Property GameOver() As Boolean
            Get
                Return _gameOver
            End Get
            Set(value As Boolean)
                _gameOver = value
            End Set
        End Property
    End Class


End Module

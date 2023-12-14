using WorldOfZuul;
namespace WorldOfZuul
{
    public class Map
    {
            // Initialize a list to store tree coordinates
        public List<List<int>> tree_coords = new();
        // 2D list representing the game map
        public List<List<Square>> this_map = new(); //2d list => [[]]
 
        // Initialize the game map with specified dimensions
        public void Initialize(int hor, int ver)  //2d list => [["f", ...], ...]
        {
            // Define possible sides for placing mines
            List<int> possible_sides = new() {0, hor-1}; //hor-1
            Random rnd = new Random();

            // Randomly select a side and row for placing mines
            int mines_side_index = rnd.Next(0,2);
            int mines_row_index = rnd.Next(3,ver-3);
            List<int> mines_row_list = new() {mines_row_index-2, mines_row_index-1, mines_row_index, mines_row_index+1, mines_row_index+2};

            // Randomly select coordinates for a central tree
            int central_tree_ver = rnd.Next(2,ver-3);
            int central_tree_hor = rnd.Next(2,hor-3);

            // Generate possible coordinates for surrounding trees
            List<List<int>> possible_tree_coords = new();
            for (int possible_row=-1; possible_row<=1; possible_row++)
            {
                for (int possible_col=-1; possible_col<=1; possible_col++)
                {
                    List<int> curr_coord = new List<int>{central_tree_hor + possible_col, central_tree_ver + possible_row};
                    if (possible_row != 0 || possible_col != 0)
                    {
                        possible_tree_coords.Add(curr_coord);
                    }
                }
            }

            List<int> central_tree_coords = new(){central_tree_hor, central_tree_ver};
            tree_coords.Add(central_tree_coords);
            
            // Select random coordinates for additional trees
            for (int i = 0; i <= 3; i++)
            {
                int rndTreeIndex = rnd.Next(0, possible_tree_coords.Count);
                List<int> rndTree = possible_tree_coords[rndTreeIndex];
                tree_coords.Add(rndTree);
                possible_tree_coords.RemoveAt(rndTreeIndex);
            }

            for (int i = 0; i < hor; i++)
            {
                List<int> bottom_row_tree_coords = new() {i, ver-1};
                tree_coords.Add(bottom_row_tree_coords);
            }

            // Populate the game map with squares based on specified conditions
            for(int row=0; row<ver; row++)
            {
                this_map.Add(new List<Square>());
                for(int column=0; column<hor; column++)
                {
                    // Water square for the top row
                    if(row == 0)
                    {
                        Square square = new('≋'); //water
                        this_map[row].Add(square);
                    }

                    // Mines square at specified locations
                    else if(mines_row_list.Contains(row) && column == possible_sides[mines_side_index])
                    {
                        Square square = new('∆'); // mines
                        this_map[row].Add(square);
                    }

                    // Random trees and plain squares for other locations
                    else
                    {
                        bool found_square = false;
                        for (int z=0; z<tree_coords.Count(); z++)
                        {
                            if (tree_coords[z][0] == column && tree_coords[z][1] == row)
                            {
                                Square square = new('♧'); // random trees
                                this_map[row].Add(square);
                                found_square = true;
                                break;
                            }
                        }
                        if (!found_square)
                        {
                            Square square = new('♦'); // plain
                            this_map[row].Add(square);
                        }
                    }
                }
            }

            // Randomly select a row for placing the mayor
            int mayorRow = rnd.Next(1, ver-2);

            // Initialize a list to store possible mayor coordinates
            List<Square> mayorPossibleCoords = new();

            // Find possible mayor coordinates in the selected row
            for(int mayorColumn=0; mayorColumn < this_map[mayorRow].Count; mayorColumn++)
            {
                Square currentSquare = this_map[mayorRow][mayorColumn];
                if (currentSquare.value == '♦' && (mayorRow != 2 || mayorColumn != 2 ))
                {
                    mayorPossibleCoords.Add(currentSquare);
                }
            }

            // Randomly select a square from the possible mayor coordinates and change its value to 'M'
            int mayorSquareIndex = rnd.Next(0, mayorPossibleCoords.Count);
            mayorPossibleCoords[mayorSquareIndex].changeValue('M');

            int captainRow = 0;

            // Initialize a list to store possible mayor coordinates
            List<Square> captainPossibleCoords = new();

            // Find possible mayor coordinates in the selected row
            for(int captainColumn=0; captainColumn < this_map[captainRow].Count; captainColumn++)
            {
                Square currentSquare = this_map[captainRow][captainColumn];
                if (currentSquare.value == '≋' )
                {
                    captainPossibleCoords.Add(currentSquare);
                }
            }

            // Randomly select a square from the possible mayor coordinates and change its value to 'M'
            int captainSquareIndex = rnd.Next(0, captainPossibleCoords.Count);
            captainPossibleCoords[captainSquareIndex].changeValue('C');
        }

        public void Print(List<int>? playerPosition)
        {
            // Print the game map, highlighting the player position if provided
            Console.WriteLine();
            for(int row=0; row < this_map.Count; row++)
            {
                for(int column=0; column < this_map[row].Count; column++)
                {
                    // Check if player position is provided
                    if (playerPosition != null)
                    {
                        //Highlight player position with a special symbol
                        if (column == playerPosition[0] && row == playerPosition[1])
                        {
                            Console.Write("🫅  "); // Player
                        }
                         // Map different square values to corresponding symbols
                        else if(this_map[row][column].value=='♧')
                        {
                            Console.Write("🌳  "); 
                        }
                        else if(this_map[row][column].value=='∆')
                        {
                            Console.Write("🏔️  ");
                        }
                        else if(this_map[row][column].value=='≋')
                        {
                            Console.Write("🌊  "); 
                        }
                        else if(this_map[row][column].value=='♦')
                        {
                            Console.Write("🟩  "); 
                        }
                        else if(this_map[row][column].value=='M')
                        {
                            Console.Write("👔  "); 
                        }
                        else if(this_map[row][column].value=='l')
                        {
                            Console.Write("🏠  ");
                        }
                        else if(this_map[row][column].value=='m')
                        {
                            Console.Write("🏪  ");
                        }
                        else if(this_map[row][column].value=='w')
                        {
                            Console.Write("🏭  ");
                        }
                        else if(this_map[row][column].value=='t')
                        {
                            Console.Write("🏛️  ");
                        }
                        else if(this_map[row][column].value=='h')
                        {
                            Console.Write("🏥  ");
                        }
                        else if(this_map[row][column].value=='e')
                        {
                            Console.Write("🏫  ");
                        }
                        else if(this_map[row][column].value=='p')
                        {
                            Console.Write("🏬  ");
                        }
                        else if(this_map[row][column].value=='c')
                        {
                            Console.Write("🏞️  ");
                        }
                        else if(this_map[row][column].value=='f')
                        {
                            Console.Write("🚒  ");
                        }
                        else if(this_map[row][column].value=='b')
                        {
                            Console.Write("💸  ");
                        }
                        else if(this_map[row][column].value=='s')
                        {
                            Console.Write("🏟️  ");
                        }
                        else if(this_map[row][column].value=='C')
                        {
                            Console.Write("🚢  ");
                        }
                        else
                            Console.Write(this_map[row][column].value);
                    }
                    else
                    {
                        // If player position is not provided, print the map without highlighting the player
                        if(this_map[row][column].value=='♧')
                        {
                            Console.Write("🌳  "); 
                        }
                        else if(this_map[row][column].value=='∆')
                        {
                            Console.Write("🏔️  ");
                        }
                        else if(this_map[row][column].value=='≋')
                        {
                            Console.Write("🌊  "); 
                        }
                        else if(this_map[row][column].value=='♦')
                        {
                            Console.Write("🟩  "); 
                        }
                        else if(this_map[row][column].value=='M')
                        {
                            Console.Write("👔  "); 
                        }
                        else if(this_map[row][column].value=='l')
                        {
                            Console.Write("🏠  ");
                        }
                        else if(this_map[row][column].value=='m')
                        {
                            Console.Write("🏪  ");
                        }
                        else if(this_map[row][column].value=='w')
                        {
                            Console.Write("🏭  ");
                        }
                        else if(this_map[row][column].value=='t')
                        {
                            Console.Write("🏛️  ");
                        }
                        else if(this_map[row][column].value=='h')
                        {
                            Console.Write("🏥  ");
                        }
                        else if(this_map[row][column].value=='e')
                        {
                            Console.Write("🏫  ");
                        }
                        else if(this_map[row][column].value=='p')
                        {
                            Console.Write("🏬  ");
                        }
                        else if(this_map[row][column].value=='c')
                        {
                            Console.Write("🏞️  ");
                        }
                        else if(this_map[row][column].value=='f')
                        {
                            Console.Write("🚒  ");
                        }
                        else if(this_map[row][column].value=='b')
                        {
                            Console.Write("💸  ");
                        }
                        else if(this_map[row][column].value=='s')
                        {
                            Console.Write("🏟️  ");
                        }
                        else if(this_map[row][column].value=='C')
                        {
                            Console.Write("🚢  ");
                        }
                        else
                            Console.Write(this_map[row][column].value);
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Change the value of a specific square using Square object
        public void Change(Square squareToChange, char newValue)
        {
            squareToChange.changeValue(newValue);
        }

        // Change the value of a specific square using its coordinates
        public void Change(List<int> squareCoords, char newValue)
        {
            // Get the Square object using the provided coordinates and change its value
            Square squareToChange = this_map[squareCoords[0]][squareCoords[1]];
            squareToChange.changeValue(newValue);
        }
    }
}
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int L = int.Parse(inputs[0]);
        int C = int.Parse(inputs[1]);
        Deska deska = new Deska(L, C);
        Robot robot = new Robot(deska);
        robot.startGame();
    }
}

class Policko
{
    public char znak { get; set; }
    public bool pruchozi = true;
    public int x { get; set; }
    public int y { get; set; }
    public int visited = 0;
    public int xT { get; set; }
    public int yT { get; set; }
}
class Deska
{
    public int L { get; set; }
    public int C { get; set; }
    public List<char> signs = new List<char>() { 'S', 'E', 'N', 'W', 'T', 'I', 'B', '@', '$', 'X', '#' };
    public Policko[,] policko { get; set; }
    public Policko start { get; set; }
    public Deska(int L, int C)
    {
        this.L = L;
        this.C = C;
        policko = new Policko[L, C];
        int tX = 0;
        int tY = 0;
        bool tB = false;
        for (int i = 0; i < L; i++)
        {
            string row = Console.ReadLine();
            for (int j = 0; j < C; j++)
            {
                policko[i, j] = new Policko();
                policko[i, j].znak = row[j];
                policko[i, j].x = i;
                policko[i, j].y = j;
                if (row[j] == 'X' || row[j] == '#')
                {
                    policko[i, j].pruchozi = false;
                }
                if (row[j] == '@')
                {
                    start = policko[i, j];
                }
                if (row[j] == 'T')
                {
                    if (tB)
                    {
                        policko[i, j].xT = tX;
                        policko[i, j].yT = tY;
                        policko[tX, tY].xT = i;
                        policko[tX, tY].yT = j;
                    }
                    tX = i;
                    tY = j;
                    tB = true;
                }

            }
        }
    }
}

class Robot
{
    public bool beer = false;
    public bool pathChanged = false;
    public int positionX { get; set; }
    public int positionY { get; set; }
    public List<string> moves = new List<string>();
    public List<string> priorities = new List<string> { "SOUTH", "EAST", "NORTH", "WEST" };
    public string direction = "SOUTH";
    public string futureDirection { get; set; }
    public bool detectLoop()
    {
        if (deska.policko[positionX, positionY].visited > 8 && !pathChanged)
        {
            return true;
        }
        return false;
    }
    public Policko budouciPolicko { get; set; }
    public void SetBudouciPolicko()
    {
        switch (direction)
        {
            case "SOUTH":
                budouciPolicko = deska.policko[positionX + 1, positionY];
                break;
            case "EAST":
                budouciPolicko = deska.policko[positionX, positionY + 1];
                break;
            case "NORTH":
                budouciPolicko = deska.policko[positionX - 1, positionY];
                break;
            case "WEST":
                budouciPolicko = deska.policko[positionX, positionY - 1];
                break;
        }
    }
    public Deska deska { get; set; }
    public Robot(Deska deska)
    {
        this.deska = deska;
    }
    public void revertDirection()
    {
        priorities.Reverse();
    }
    public void startGame()
    {
        // Write an answer using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        positionX = deska.start.x;
        positionY = deska.start.y;
        while (move())
        {
            if (detectLoop())
            {
                Console.WriteLine("LOOP");
                break;
            }
            deska.policko[positionX, positionY].visited++;
        }
    }
    public bool move()
    {
        SetBudouciPolicko();
        if (!signs()) return false;
        if (budouciPolicko.pruchozi)
        {
            positionX = budouciPolicko.x;
            positionY = budouciPolicko.y;
            moves.Add(direction);
            Console.WriteLine(direction);
            direction = futureDirection;
        }
        else
        {
            findWay();
        }
        return true;
    }
    public void findWay()
    {
        for (int i = 0; i < priorities.Count; i++)
        {
            direction = priorities[i];
            SetBudouciPolicko();
            signs();
            if (budouciPolicko.pruchozi)
            {
                positionX = budouciPolicko.x;
                positionY = budouciPolicko.y;
                moves.Add(direction);
                Console.WriteLine(direction);
                direction = futureDirection;
                break;
            }
        }
    }


    public bool signs()
    {
        futureDirection = direction;
        switch (budouciPolicko.znak)
        {
            case 'S':
                futureDirection = "SOUTH";
                break;
            case 'E':
                futureDirection = "EAST";
                break;
            case 'N':
                futureDirection = "NORTH";
                break;
            case 'W':
                futureDirection = "WEST";
                break;
            case 'I':
                revertDirection();
                break;
            case 'T':
                budouciPolicko = deska.policko[budouciPolicko.xT, budouciPolicko.yT];
                break;
            case 'B':
                beer = !beer;
                break;
            case 'X':
                if (beer)
                {
                    budouciPolicko.znak = 'C';
                    budouciPolicko.pruchozi = true;
                    pathChanged = true;
                }
                break;
            case 'C':
                pathChanged = false;
                break;
            case '#':
                break;
            case '@':
                break;
            case '$':
                moves.Add(direction);
                for (int i = 0; i < moves.Count; i++)
                {
                    Console.WriteLine(moves[i]);
                }
                return false;
            default:
                break;
                // vsechny mozny pripadz

        }
        return true;
    }
}
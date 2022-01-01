using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2021Project
{
    class InvaildValueException : Exception
    {
        public enum State { ShapeISNotMatch = 0, PloidyISNotMatch, OddNumberOfCell}
        public InvaildValueException(State state)
        {
            Msg = state;
            switch (state)
            {
                case State.ShapeISNotMatch:
                    explanation = "Shape is not match";
                    break;
                case State.PloidyISNotMatch:
                    explanation = "Ploidy is not match";
                    break;
                case State.OddNumberOfCell:
                    explanation = "The number of cells is odd.";
                    break;
            }


        }

        public State Msg;
        public string explanation;
    }

    class CellIO
    {
      
        public List<Cell> ParseCell(string filePath)
        {
            
            StreamReader sr = new StreamReader(filePath);
            List<Cell> result = new List<Cell>();

            while (!sr.EndOfStream)
            {
                // 한 줄씩 읽어온다.
                string buf = sr.ReadLine();
                if (buf[1].Equals("!")) {continue;};

                // 쉼표( , )를 기준으로 데이터를 분리한다.
                string[] data = buf.Split(',');
                string[] shapeStr= data[0].Split('/');
                int[] shape = new int[shapeStr.Length];
                
                for(int i = 0; i < shapeStr.Length; i++)
                {
                    shape[i] = Convert.ToInt32(shapeStr[i]);
                }
                
                Cell temp = new Cell(2, shape);
                temp.SetGenType(data[1],data[2]);
                result.Add(temp);
            }

            sr.Close();
            return result;
        }

        private void PrintVarticaly(StreamWriter sw, string left, string right)
        {
            for (int i = 0; i < left.Length; i++)
            {
                sw.WriteLine(left[i] + "|" + right[i]);
            }
        }

        private void PrintVarticaly(string left, string right)
        {
            for(int i = 0; i < left.Length; i++)
            {
                Console.WriteLine(left[i] + "|" + right[i]);
            }
        }

        public void PrintGentype(Cell input)
        {
            if(input.Ploidy == 1)
            {
                for(int i = 0; i < input.Left.Length; i++)
                {
                    Console.WriteLine(input.Left[i]);
                }
            }
            else
            {
                int ptr = 0;
                for (int i = 0; i < input.Shape.Length; i++)
                {
                    string bufL = input.Left.Substring(ptr, input.Shape[i]);
                    string bufR = input.Right.Substring(ptr, input.Shape[i]);

                    MatchCollection mcL = Regex.Matches(bufL, "[A-Z]");
                    MatchCollection mcR = Regex.Matches(bufR, "[A-Z]");

                    if(mcL.Count >= mcR.Count)
                    {
                        PrintVarticaly(bufL, bufR);
                    }
                    else
                    {
                        PrintVarticaly(bufR, bufL);
                    }
                    ptr += input.Shape[i];
                    Console.WriteLine("---");
                }
            }
        }

        public void WriteGenTypeToFile(StreamWriter sw, Cell input)
        {
            if (input.Ploidy == 1)
            {
                for (int i = 0; i < input.Left.Length; i++)
                {
                    sw.WriteLine(input.Left[i]);
                }
            }
            else
            {
                int ptr = 0;
                for (int i = 0; i < input.Shape.Length; i++)
                {
                    string bufL = input.Left.Substring(ptr, input.Shape[i]);
                    string bufR = input.Right.Substring(ptr, input.Shape[i]);

                    MatchCollection mcL = Regex.Matches(bufL, "[A-Z]");
                    MatchCollection mcR = Regex.Matches(bufR, "[A-Z]");

                    if (mcL.Count >= mcR.Count)
                    {
                        PrintVarticaly(sw,bufL, bufR);
                    }
                    else
                    {
                        PrintVarticaly(sw, bufR, bufL);
                    }
                    ptr += input.Shape[i];
                    sw.WriteLine("---");
                }
            }

        }
    }


    class Cell
    {
        
        public int Ploidy;
        private int NumberOfGen;

        public int[] Shape;
        public string Left; 
        public string Right;

        public Cell(int ploidy, int[] shape)
        {
            Shape = shape;
            Ploidy = ploidy;
            
            foreach(int i in shape)
            {
                NumberOfGen += i;
            }

        }

        public void SetGenType(string leftGen, string rightGen)
        {

            if(Ploidy != 2)
            {
                throw new InvaildValueException(InvaildValueException.State.PloidyISNotMatch);
            }

            Left = leftGen;
            Right = rightGen;
        }

        public void SetGenType(string Gen)
        {
         
            if (Ploidy != 1)
            {
                throw new InvaildValueException(InvaildValueException.State.PloidyISNotMatch);
            }

            Left = Gen;
        }
        private List<bool[]> GetCombination(int num)
        {

            List<bool[]> result = new List<bool[]>();

            bool[] init = new bool[num];
            for(int i = 0; i < num; i++)
            {
                init[i] = false;
            }
            result.Add(init);

            List<bool[]> tmp = result;

            for (int i = 0; i < num; i++)
            {
                tmp = GetCombination(tmp, num);
                result.AddRange(tmp);
            }

            return result;
        }

        private List<bool[]> GetCombination(List<bool[]> input, int Num)
        {
            List<bool[]> result = new List<bool[]>();
            
            for (int i = 0; i < input.Count;  i++)
            {
                int ptr = 0;
                bool[] tmp = input[i];
                while (ptr < Num && tmp[ptr] == false){ ptr++; }

                for(int j = ptr -1; j >= 0; j--)
                {
                    tmp = (bool[])input[i].Clone();
                    tmp[j] = true;
                    result.Add(tmp);
                }
            }

            return result;
        }

        public List<Cell> meiosis()
        {
            List<bool[]> combination = GetCombination(Shape.Length);
            string[] _case = new string[combination.Count];

            for (int k = 0; k < combination.Count; k++)
            {
                //생식세포 분열 시뮬레이션
                int ptr = 0;
                for(int i = 0; i < Shape.Length; i++)
                {
                    if(combination[k][i] == true)
                    {
                        _case[k] = _case[k] + this.Left.Substring(ptr, Shape[i]);
                    }
                    else
                    {
                        _case[k] = _case[k] + this.Right.Substring(ptr, Shape[i]);
                        
                    }
                    ptr += Shape[i];
                }
                
            }

            //중복제거
            List<Cell> daughtercells = new List<Cell>();
            _case = _case.Distinct().ToArray();
           
            //구한 유전자형의 경우로 세포 객체 생성해서 딸세포 리스트에 추가
            foreach(string val in _case)
            {
                Cell cell = new Cell(1, Shape);
                cell.SetGenType(val);
                daughtercells.Add(cell);
            }

            //만들어진 딸세포 리스트 반환
            return daughtercells;
        }

        static public List<Cell> MateCells(Cell cell1, Cell cell2)
        {
            List<Cell> dautherOfCell1 = cell1.meiosis();
            List<Cell> dautherOfcell2 = cell2.meiosis();

            List<Cell> result = new List<Cell>();

            foreach(Cell val1 in dautherOfCell1)
            {
                foreach(Cell val2 in dautherOfcell2)
                {
                    if (!cell1.Shape.SequenceEqual(cell2.Shape)) { throw new InvaildValueException(InvaildValueException.State.ShapeISNotMatch); }

                    Cell tmp = new Cell(2, cell1.Shape);
                    tmp.SetGenType(val1.Left, val2.Left);
                    result.Add(tmp);
                }
            }

            return result;
        }
        
    }

    class Program
    {
        const string InputFilePath = @".\input.txt";
        const string OutputFilePath = @".\Output.txt";

        static void Main(string[] args)
        {
        
            List<Cell> mother= new List<Cell>();
            List<Cell> father = new List<Cell>();
            CellIO iO = new CellIO();

            List<Cell> wholeCells = iO.ParseCell(InputFilePath);
            bool IsCellOfmather = false;

            if(wholeCells.Count % 2 != 0)
            {
                throw new InvaildValueException(InvaildValueException.State.OddNumberOfCell);
            }

            for(int i = 0; i < wholeCells.Count; i++)
            {
                if(IsCellOfmather == true)
                {
                    mother.Add(wholeCells[i]);
                    IsCellOfmather = false;
                }
                else
                {
                    father.Add(wholeCells[i]);
                    IsCellOfmather = true;
                }
            }

            StreamWriter sw = new StreamWriter(OutputFilePath);
            

            for (int i = 0; i < mother.Count; i++)
            {
                List<Cell> result = new List<Cell>();

                Console.WriteLine("CASE: " + i.ToString() + " ----------------------");
                iO.PrintGentype(mother[i]);
                Console.WriteLine("*");
                iO.PrintGentype(father[i]);
                Console.WriteLine(":");
                
                Console.WriteLine();
                result = Cell.MateCells(mother[i], father[i]);
                foreach (Cell val in result)
                {
                    iO.PrintGentype(val);
                    Console.WriteLine();
                }

                sw.WriteLine("CASE: " + i.ToString() + " ----------------------");
                iO.WriteGenTypeToFile(sw, mother[i]);
                sw.WriteLine("*");
                iO.WriteGenTypeToFile(sw, father[i]);
                sw.WriteLine(":");

                sw.WriteLine();
                foreach (Cell val in result)
                {
                    iO.WriteGenTypeToFile(sw, val);
                    sw.WriteLine();
                }


            }
            sw.Close();
        }
    }
}

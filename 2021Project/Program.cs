using System;
using System.Collections.Generic;
namespace _2021Project
{
    
    class Breeder
    {

    }

    class Cell
    {
        //비분리는 고려하지 않음
        //Shape -> 염색체가 어떻게 구성됐나 "1/1/1/2/1" 나중에 private로 바꾸기

        public int Ploidy;
        private int NumberOfGen;

        int[] Shape;
        char[] left; 
        char[] right;

        public Cell(int ploidy, int[] shape)
        {
            Shape = shape;
            Ploidy = ploidy;
            
            foreach(int i in shape)
            {
                NumberOfGen += i;
            }

            if(Ploidy == 1)
            {
                left = new char[NumberOfGen];
            }
            else if(Ploidy == 2)
            {
                left = new char[NumberOfGen];
                right = new char[NumberOfGen];
            }

        }

        public void SetGenType(char[] leftGen, char[] rightGen)
        {

            if(Ploidy != 2)
            {
                throw new Exception();
            }

            left = leftGen;
            right = rightGen;
        }

        public void SetGenType(char[] Gen)
        {
         
            if (Ploidy != 1)
            {
                throw new Exception();
            }

            left = Gen;
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
            List<Cell> daughtercells = new List<Cell>();
            List<bool[]> combination = GetCombination(Shape.Length);

            for(int k = 0; k < combination.Count; k++)
            {
                Cell _case = new Cell(1, Shape);
                int ptr = 0;
                for(int i = 0; i < Shape.Length; i++)
                {
                    for(int j = 0; j < Shape[i]; j++)
                    {
                        //true이면 left
                        if(combination[k][i] == true)
                        {
                            _case.left[ptr] = this.left[ptr];
                        }
                        else
                        {
                            _case.left[ptr] = this.right[ptr];
                        }
                        ptr++;
                    }
                }

                daughtercells.Add(_case);
                
            }

            return daughtercells;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Cell m = new Cell(2, new int[] { 1, 2 });
            m.SetGenType(new char[] { 'A', 'b', 'D', }, new char[] { 'A', 'b', 'd', });
           // Cell f = new Cell(2, new int[] { 1, 1, 1 });
            List<Cell> DC = m.meiosis();
        }
    }
}

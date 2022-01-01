using System;
using System.Collections.Generic;
using System.Linq;

namespace _2021Project
{


    class Cell
    {
        
        public int Ploidy;
        private int NumberOfGen;

        int[] Shape;
        string left; 
        string right;

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
                throw new Exception();
            }

            left = leftGen;
            right = rightGen;
        }

        public void SetGenType(string Gen)
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
                        _case[k] = _case[k] + this.left.Substring(ptr, Shape[i]);
                    }
                    else
                    {
                        _case[k] = _case[k] + this.right.Substring(ptr, Shape[i]);
                        
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


    }

    class Program
    {
        static void Main(string[] args)
        {
            Cell m = new Cell(2, new int[] { 1, 2 });
            m.SetGenType("Abd","AbD");
           // Cell f = new Cell(2, new int[] { 1, 1, 1 });
            List<Cell> DC = m.meiosis();
        }
    }
}

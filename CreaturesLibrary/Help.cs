using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreaturesLibrary
{
    public class Help
    {
        /// <summary>
        /// Рандомайзер.
        /// </summary>
        public static Random random = new Random();

        /// <summary>
        /// Создает имя с длиной в заданных грацницах. 
        /// АЛЬТЕРНАТИВА: СОЗДАВАТЬ СТРОКУ ЧЕРЕЗ StringBuilder.
        /// </summary>
        /// <param name="min"> Минимальная длина имени. </param>
        /// <param name="max"> Максимальная длина имени. </param>
        /// <returns></returns>
        public static string BuildName(int min, int max)
        {
            string name = "";
            name += (char)random.Next('A', 'Z' + 1);
            int chance = random.Next(min - 1, max);
            for (int i = 0; i < chance; i++)
            {
                name += (char)random.Next('a', 'z' + 1);
            }
            return name;
        }
        /*StringBuilder name = new StringBuilder(size);
        name.Append((char)random.Next('A', 'Z' + 1));
        for (int i = 1; i < size; i++)
        {
        name.Append((char)random.Next('a', 'z' + 1));
        return name.ToString();
        }*/
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using CreaturesLibrary;
using System.Xml;
using System.Runtime.Serialization;
using System.Security;
using System.IO;

namespace CreatureAnalyzer
{
    class Program
    {
        /// <summary>
        /// Путь для нахождения файла. откуда будем дессериализовывать объекты.
        /// </summary>
        public const string path = "../../../creatures.xml";

        static void Main(string[] args)
        {
            // Создаем лист созданий.
            List<Creature> creatures = new List<Creature>();
            try
            {
                // Десериализуем в лист созданий.
                using (XmlReader xmlReader = XmlReader.Create(path))
                {
                    DataContractSerializer disser = new DataContractSerializer(typeof(List<Creature>));
                    creatures = (List<Creature>)disser.ReadObject(xmlReader);
                }
            }
            // Ловим всякого рода исключения.
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SecurityException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            /*
            // Создаем сериализатор типа листа созданий.
                XmlSerializer formatter = new XmlSerializer(typeof(List<Creature>));
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    // Дессиреализуем файл и первращаем хмл в лист созданий.
                    creatures = (List<Creature>)formatter.Deserialize(fs);

                    // Выводим инйормацию о каждом создании на эеран. 
                    foreach (Creature creature in creatures)
                    {
                        Console.WriteLine(creature.ToString());
                    }
                }
            */

            // При помощи линков выводим количество плавающих созданий: Count(выборка по способу передвижения).
            Console.WriteLine("\nNumber of swimming creatures: " + creatures.Count(creature => creature.MovementType == 0) + "\n");

            // При помощи линков выводим 10 первых элементов(Take(n-первых элементов)) из новой коллекции, 
            // отсортированной по убыванию здоровья (OrderByDescending()).
            Console.WriteLine("10 первых элементов из коллекции, отсортированной по убыванию здоровья.\n");
            Console.WriteLine(String.Join(Environment.NewLine, creatures.OrderByDescending((creature) => creature.Health).Take(10).ToList())
                + Environment.NewLine);
            /*АЛЬТЕРНАТИВОЧКА 2 ПУНКТА.
            Console.WriteLine(String.Join(Environment.NewLine, creatures.OrderBy((creature) => creature.Health).Reverse().Take(10).ToList()));*/

            // Сгруппировать по типу передвижения (GroupBy()), в каждой из подгрупп выполнить 
            // переопределенное умножение созданий друг с другом (Aggregate), вывести на экран результат.
            Console.WriteLine("Сгруппированная по способам передвижения и перемноженная:\n");
            Console.WriteLine(String.Join(Environment.NewLine, creatures.GroupBy(creature => creature.MovementType)
                .Select(creature => creature.Aggregate((cr1, cr2) => cr1 * cr2)).ToList()) + Environment.NewLine);


            // Сгруппировать по типу передвижения (GroupBy()), в каждой из подгрупп выполнить 
            // переопределенное умножение созданий друг с другом (Aggregate), 
            // при помощи линков выводим 10 первых элементов(Take(n-первых элементов)) из новой коллекции, 
            // отсортированной по убыванию здоровья (OrderByDescending()).
            Console.WriteLine("Сгруппированная по способам передвижения и перемноженная\n + " +
                "отсортированнаые по убыванию Health 10 элементов:\n");
            Console.WriteLine(String.Join(Environment.NewLine, creatures.GroupBy(creature => creature.MovementType)
                .Select(creature => creature.Aggregate((cr1, cr2) => cr1 * cr2)).ToList()
                .OrderByDescending((creature) => creature.Health).ThenBy(cr => cr.Name).Take(10).ToList()));
        }
    }
}

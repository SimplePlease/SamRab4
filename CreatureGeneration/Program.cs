using System;
using System.Collections.Generic;
using System.Linq;
using CreaturesLibrary;
using System.IO;
using System.Security;
using System.Xml;
using System.Runtime.Serialization;

namespace CreatureGenerator
{
    class Program
    {
        /// <summary>
        /// Строковая констаната с путем к файлу, в который будем все записывать.
        /// </summary>
        public const string path = "../../../creatures.xml";
        static void Main(string[] args)
        {
            // Лист с объектами тип Creatures.
            List<Creature> creatures = new List<Creature>();

            // Создаем 30 объектов.
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    // С равной вероятность делаем создание плавающим, ходячим или летающим.
                    int chance = Help.random.Next(0, 3);

                    // Создание объекта, добавление его в лист.
                    creatures.Add(new Creature(Help.BuildName(6, 10), (MovementTypeEnum.MovementType)chance,
                        Help.random.Next(0, 10) + Help.random.NextDouble()));

                    // Вывод свежесозданных объектов.
                    Console.WriteLine($"{creatures.Last().ToString()},");
                }
                // При возникновении непредвиденных исключений, пересоздаем ъект, выводим информацию об ошибке.
                catch (Exception ex)
                {
                    Console.WriteLine($"Problem with {i + 1} creture: " + ex.Message);
                    i--;
                }
            }

            try
            {
                // Сериализуем List объектов.
                using (XmlWriter xmlWriter = XmlWriter.Create(path))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(List<Creature>));
                    ser.WriteObject(xmlWriter, creatures);
                }

                /*
                 * А вообще, уусловие неточное - можно было просто по-человечески сделать setы публичными, ведь
                 * сериализация и так подразумевает возможную утечку данных.
                    // Создаем сериализатор типа Листа Созданий.
                    XmlSerializer formatter = new XmlSerializer(typeof(List<Creature>));
                    // Создаем поток, сериализуем лист объектов.
                    StreamWriter sw = new StreamWriter(path);
                    formatter.Serialize(sw, creatures);
                    sw.Close();

                 * АЛЬТЕРНАТИВОЧКА к альтернативочке.
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        ser.Serialize(fs, creatures);
                    }
                 */
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
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CreaturesLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Security;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;

namespace CreatureTest
{
    /// <summary>
    /// Класс тестирующего модуля для проверки работоспособности программы.
    /// </summary>
    [TestClass]
    public class CreatureTest
    {
        /// <summary>
        /// Строковая константа с путем к файлу с тестом работы сериализации/десериалзации.
        /// </summary>
        public const string testSerDis = "../../../test.xml";

        /// <summary>
        /// Тестирование переопределенной операции умножения.
        /// </summary>
        [TestMethod]
        public void TestOperator()
        {
            // Создаем объекты класса Creature для тестирования.
            Creature a = new Creature("Andrey", MovementTypeEnum.MovementType.Swimming, 7);
            Creature b = new Creature("Malika", MovementTypeEnum.MovementType.Swimming, 9);

            Creature c = new Creature("afafafafaafafafafa", MovementTypeEnum.MovementType.Walking, 1.949430);
            Creature d = new Creature("qwerty", MovementTypeEnum.MovementType.Walking, 1.40544);

            Creature e = new Creature("Maman", MovementTypeEnum.MovementType.Flying, 8.88888);
            Creature f = new Creature("Father", MovementTypeEnum.MovementType.Flying, 9.999999);
            Creature g = new Creature("Brother", MovementTypeEnum.MovementType.Flying, 9.999999);

            // Проверяем наличие исключений при умножении животных с различными способами передвижения.
            Assert.ThrowsException<ArgumentException>(() => a * c);
            Assert.ThrowsException<ArgumentException>(() => a * e);
            Assert.ThrowsException<ArgumentException>(() => b * e);

            // Генерируем фактические результаты работы программы.
            // Проверка, правильно ли берется первая половины имени у первого множителя, если длина имен одинаковая.
            Creature result1 = a * b;
            Creature result2 = b * a;
            Creature result3 = c * c;
            // Ожидаемые результаты.
            Creature expected1 = new Creature("Andika", MovementTypeEnum.MovementType.Swimming, (7 + 9) / 2.0);
            Creature expected2 = new Creature("Malrey", MovementTypeEnum.MovementType.Swimming, (7 + 9) / 2.0);
            Creature expected3 = new Creature("afafafafaafafafafa", MovementTypeEnum.MovementType.Walking, 1.949430);

            // Проверяем работу с анаграмами, умножение трех подряд, два из которых одинаковы.
            Creature result4 = d * c;
            Creature result5 = c * d;
            Creature result6 = c * c * d;
            // Ожидаемые результаты.
            Creature expected4 = new Creature("afafafafarty", MovementTypeEnum.MovementType.Walking, (1.40544 + 1.949430) / 2.0);
            Creature expected5 = new Creature("afafafafarty", MovementTypeEnum.MovementType.Walking, (1.40544 + 1.949430) / 2.0);
            Creature expected6 = expected5;

            // Проверка того, что умножение трез подряд работает правильно.
            Creature result7 = e * f;
            Creature result8 = result7 * g;
            Creature result9 = e * f * g;
            // Ожидаемые результаты.
            Creature expected7 = new Creature("Fatan", MovementTypeEnum.MovementType.Flying, (8.88888 + 9.999999) / 2.0);
            Creature expected8 = new Creature("Brotan", MovementTypeEnum.MovementType.Flying,
                (((8.88888 + 9.999999) / 2.0) + 9.999999) / 2.0);
            Creature expected9 = new Creature("Brotan", MovementTypeEnum.MovementType.Flying,
                (((8.88888 + 9.999999) / 2.0) + 9.999999) / 2.0);


            // Сравниваем то, что планировали получить с тем, что выдает программа.
            Assert.AreEqual(expected1, result1);
            Assert.AreEqual(expected2, result2);
            Assert.AreEqual(expected3, result3);
            Assert.AreEqual(expected4, result4);
            Assert.AreEqual(expected5, result5);
            Assert.AreEqual(expected6, result6);
            Assert.AreEqual(expected7, result7);
            Assert.AreEqual(expected8, result8);
            Assert.AreEqual(expected9, result9);
        }

        /// <summary>
        /// Тесттирование корректности метода ToString().
        /// </summary>
        [TestMethod]
        public void TestToString()
        {
            // Создаем объекты класса Creature для тестирования.
            Creature a = new Creature("Andrey", MovementTypeEnum.MovementType.Swimming, 7);
            Creature g = new Creature("Brother", MovementTypeEnum.MovementType.Swimming, 9.99);
            Creature c = new Creature("Somebody", MovementTypeEnum.MovementType.Swimming, 9.999999);
            Creature d = new Creature();

            // Проверка 2 примитивных объектов.
            string expected1 = "Swimming creature Andrey: Health = 7,000";
            string expected2 = "Swimming creature Brother: Health = 9,990";
            // Проверка работы метода от только уноженных объектов.
            string expected3 = "Swimming creature Brotrey: Health = 8,495";
            // Проверка, округляет ли длинные числа с периодом.
            string expected4 = "Swimming creature Somebody: Health = 10,000";
            // Проверка объекта, созданного конструктором без параметров.
            string expected5 = "Swimming creature : Health = 0,000";

            // Сравниваем то, что планировали получить с тем, что выдает программа.
            Assert.AreEqual(expected1, a.ToString());
            Assert.AreEqual(expected2, g.ToString());
            Assert.AreEqual(expected3, (a * g).ToString());
            Assert.AreEqual(expected4, c.ToString());
            Assert.AreEqual(expected5, d.ToString());
        }

        /// <summary>
        /// Тест, который проверяет корректность сеериализации и дессериализация.
        /// </summary>
        [TestMethod]
        public void TestSerialization()
        {
            // Создаем лист созданий, заполняем 30-ью эелементами, создаем сериализатор, сериализуем.
            // (код, полностью идентичный тому, что в CreatureAnalyzer.)
            List<Creature> creatures = new List<Creature>();

            for (int i = 0; i < 30; i++)
            {
                int chance = Help.random.Next(0, 3);
                creatures.Add(new Creature(Help.BuildName(6, 10), (MovementTypeEnum.MovementType)chance,
                    Help.random.Next(0, 10) + Help.random.NextDouble()));
            }

            using (XmlWriter xmlWriter = XmlWriter.Create(testSerDis))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(List<Creature>));
                ser.WriteObject(xmlWriter, creatures);
            }

            // Создаем лист десериализованных созданий.
            List<Creature> creaturesDeserialized = new List<Creature>();

            // Дессериализуем в creaturesDeserialized, сравниваем, идентичны ли 2 листа: 
            // тот, который был создан и тот, что был вынут из файла.
            // десериализуем в лист созданий.
            using (XmlReader xmlReader = XmlReader.Create(testSerDis))
            {
                DataContractSerializer disser = new DataContractSerializer(typeof(List<Creature>));
                creaturesDeserialized = (List<Creature>)disser.ReadObject(xmlReader);
                Assert.IsTrue(Enumerable.SequenceEqual(creatures, creaturesDeserialized));
            }

            // После запуска теста стало очевидно, что десериализация  работает: 
            // просериализуем две одинаковые коллекции и сравним тексты из xml-файла.
            string a = File.ReadAllText(testSerDis);
            using (XmlWriter xmlWriter = XmlWriter.Create(testSerDis))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(List<Creature>));
                ser.WriteObject(xmlWriter, creaturesDeserialized);
            }
            string b = File.ReadAllText(testSerDis);
            Assert.AreEqual(a, b);
        }
    }
}

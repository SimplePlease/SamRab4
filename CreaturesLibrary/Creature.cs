using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace CreaturesLibrary
{
    [DataContract]
    public class Creature
    {
        /// <summary>
        /// Публичный конструктор, создающий создание.
        /// </summary>
        /// <param name="name"> Имя объекта. </param>
        /// <param name="movementType"> Способ передвижения объекта. </param>
        /// <param name="health"> Здоровье объекта. </param>
        public Creature(string name, MovementTypeEnum.MovementType movementType, double health)
        {
            // Если диапозон здоровья не подходит, то бросаем исключение.
            if (health > 10 || health <= 0)
            {
                throw new Exception("Wrong Health.");
            }

            Name = name;
            MovementType = movementType;
            Health = health;
        }

        /// <summary>
        /// Конструктор без прааметров, нужый для того, чтобы класс был xml-сериализуемый.
        /// </summary>
        public Creature() { }

        /// <summary>
        /// Строковое автосвойство, хранящее имя объекта.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Автосвойство, хранящее информацию о способе передвижения объекта.
        /// </summary>
        [DataMember]
        public MovementTypeEnum.MovementType MovementType { get; set; }

        /// <summary>
        /// Вещественное автосвойство, хранящее информацию о здоровье создания.
        /// </summary>
        [DataMember]
        public double Health { get; set; }

        /// <summary>
        /// Переопределенный стрококвый метод, который возвращает информаци об объекте.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{MovementType} creature {Name}: Health = {Health.ToString("f3")}";
        }

        /// <summary>
        /// Переопределенный оператор умножения. Работает по следующему принципу: 
        /// к половине имени животного с более длинным именем прибавляет половину имени животного 
        /// сболлее коротким именем, здоровье - среднее арифметическое, тип передвижния как у множителей. 
        /// Возвращает новый объект.
        /// </summary>
        /// <param name="creature1"> Первый объект. </param>
        /// <param name="creature2"> Второй объект. </param>
        /// <returns></returns>
        public static Creature operator *(Creature creature1, Creature creature2)
        {
            if (creature1.Name.Length >= creature2.Name.Length)
            {
                if (creature1.MovementType == creature2.MovementType)
                {
                    return new Creature(creature1.Name.Substring(0, (int)Math.Ceiling((double)creature1.Name.Length / 2.0))
                        + creature2.Name.Substring((int)Math.Ceiling((double)creature2.Name.Length / 2.0)),
                        creature1.MovementType, (creature1.Health + creature2.Health) / 2.0);
                }
                else
                {
                    // Если у животных разный тип ередвижения, то выбрасываем исключение с соответствующим текстом.
                    throw new ArgumentException($"{creature1.MovementType.ToString()} can't be with " +
                        $"{creature2.MovementType.ToString()} creature. Nature isn't romantic.");
                }

            }
            else
            {
                if (creature1.MovementType == creature2.MovementType)
                {
                    return new Creature(creature2.Name.Substring(0, (int)Math.Ceiling((double)creature2.Name.Length / 2.0))
                        + creature1.Name.Substring((int)Math.Ceiling((double)creature1.Name.Length / 2.0)),
                        creature2.MovementType, (creature1.Health + creature2.Health) / 2.0);
                }
                else
                {
                    // Если у животных разный тип ередвижения, то выбрасываем исключение с соответствующим текстом.
                    throw new ArgumentException($"{creature1.MovementType.ToString()} can't be with " +
                        $"{creature2.MovementType.ToString()} creature. Nature isn't romantic.");
                }
            }
        }

        /// <summary>
        /// Создаем необходимый нам Equals для правильной работы AreEqual в тестах.
        /// Сравнивает по всем свойствам объекта сразу.
        /// АЛЬТЕРНАТИВОЧКА: реализация через сравнение ToString().
        /// </summary>
        /// <param name="other"> Объект, с которым свравнивают.</param>
        /// <returns></returns>
        public bool Equals(Creature other)
        {
            // Возвращаем true, если значения совпадают.
            return Name.Equals(other.Name) && MovementType.Equals(other.MovementType) && Health.Equals(other.Health);
        }

        /// <summary>
        /// Переопределеныый метод сравнние для объекта.
        /// </summary>
        /// <param name="obj"> Объект, скоторым сравнивают. </param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Creature);
        }

    }
}

/*
 * @author Artem Kudrya
 * @date 05.11.2016
 * @see Назначение: Программа состоит из трёх классов:
 *  Student - который содержит информацию о студенте
 *  Group - который содержит массив студентов
 *  Program - который содержит main
 */ 

using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Интерфейся для работы с массивом, включает в себя функцию вывода массива
/// </summary>
public interface JobInArray
{
    void showArray();
}

namespace Zadanie_1
{
    /** <remarks>
     * Класс Student содержит поля для хранения информации о студенте
     * name - Имя студента
     * name_group - имя группы, в которой находится данный студент
     * age - возвраст студента
     * pol - пол студента
     * </remarks> */
     [Serializable]
    class Student : ICloneable
    {
        //Кол-во экземпляров данного класса
        public static int kolCallStudent = 0;

        private string name;
        private string name_Group;
        private int age;
        private char pol;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Student()
        {
            kolCallStudent++;

            this.name = "###";
            this.name_Group = "###";
            this.age = 18;
            this.pol = '#';
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_name_Group"></param>
        /// <param name="_age"></param>
        /// <param name="_pol"></param>
        public Student(string _name, string _name_Group, int _age, char _pol)
        {
            kolCallStudent++;

            this.name = _name;
            this.name_Group = _name_Group;
            this.age = _age;
            this.pol = _pol;
        }

        public override string ToString()
        {
            return "Name: " + name + " Group: " + name_Group + " Age: " + age + " Pol: " + pol;
        }

        public object Clone()
        {
            return new Student { Name = this.name, Name_Group = this.name_Group, Age = this.age, Pol = this.pol };
        }

        #region Get and Set
        public string Name {
            get
            {
                return name;
            }
            set
            {
                if (value != null)
                {
                    this.name = value;
                }
            }
        }
        public string Name_Group {
            get
            {
                return name_Group;
            }
            set
            {
                if (value != null)
                {
                    this.name_Group = value;
                }
            }
        }
        public int Age {
            get
            {
                return age;
            }
            set
            {
                if (value >= 0)
                {
                    this.age = value;
                }
            }
        }
        public char Pol {
            get
            {
                return pol;
            }
            set
            {
                this.pol = value;
            }
        }
        #endregion
    }

    /** <remarks>
     * Класс расширяется от студента до группы студентов, и содержит массив студентов
     * </remarks> */
     [Serializable]
    class Group : Student, JobInArray, ICloneable, IEnumerator, IComparer
    {
        //Кол-во экземпляров данного класса
        public static int kolCallGroup = 0;

        public Student[] groupArr;

        int position = -1;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public Group()
        {
            kolCallGroup++;

            groupArr = new Student[0];
        }

        public Group(int size)
        {
            kolCallGroup++;

            groupArr = new Student[size];
        }

        public Group(string name, string nameGR, int age, char pol)
        {
            kolCallGroup++;

            Student buff = new Student(name, nameGR, age, pol);
            groupArr = new Student[1];
            groupArr[0] = buff;
        }

        public Group(Group[] list)
        {
            groupArr = list;
        }

        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            if (position == groupArr.Length - 1)
            {
                Reset();
            }

            position++;
            return true;
        }

        public void Reset()
        {
            position = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return groupArr[position];
                }
                catch
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Функция добавления нового элемента в массив
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nameGR"></param>
        /// <param name="age"></param>
        /// <param name="pol"></param>
        public void addStudent(string name, string nameGR, int age, char pol)
        {
            Array.Resize<Student>(ref groupArr, groupArr.Length + 1);
            int sizeGR = groupArr.Length;
            Student buff = new Student(name, nameGR, age, pol);
            groupArr[sizeGR-1] = buff;
        }

        /// <summary>
        /// Вывод массива
        /// </summary>
        public void showArray()
        {
            try
            {
                Console.WriteLine("Size group: " + groupArr.Length);
                foreach (Student i in groupArr)
                {
                    Console.WriteLine("Name: "+i.Name+" Group: "+i.Name_Group+" Age: "+i.Age+" Pol: "+i.Pol);
                }
            } catch (NullReferenceException e)
            {
                Console.WriteLine(e);
            }
        }

        public object Clone()
        {
            return groupArr;
        }

        public int Compare(object x, object y)
        {
            return 0;
        }

        public void saveInFile(Student group)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            Stream fstream = new FileStream("lol", FileMode.Create, FileAccess.Write, FileShare.None);
            binFormat.Serialize(fstream, group);
            Console.ReadKey();
        }

        public Group loadInFile()
        {
            Group buff = new Group();
            BinaryFormatter binFormat = new BinaryFormatter();
            Stream fstream = new FileStream("lol", FileMode.Open);
            buff = (Group)binFormat.Deserialize(fstream);
            return buff;
        }

        /// <summary>
        /// Переопределяем toString, на перевод всего массива в string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";
            foreach (Student i in groupArr)
            {
                result += "Name: " + i.Name + " Group: " + i.Name_Group + " Age: " + i.Age + " Pol: " + i.Pol;
            }
            return result;
        }
    }

    /** <remarks>
     * Главный класс, который содержит main, и отвечает за запуск всего приложения
     * </remarks> */
    class Program
    {
        /// <summary>
        /// Выполнение программы начинается с Main
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Group group = new Group();
            group.addStudent("Lol", "P-33", 18, 'M');
            group.addStudent("Lol2", "P-33", 18, 'M');
            group.addStudent("Lol3", "P-33", 18, 'M');
            group.showArray();
            group.saveInFile(group);
            Console.ReadKey();
        }
    }
}

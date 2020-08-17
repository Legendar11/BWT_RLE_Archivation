//https://studfiles.net/preview/2746750/page:2/
using System; // подключение библиотеки для общей работы
using System.Collections.Generic; // подключение библиотеки для работы с коллекциями
using System.Drawing; // подключение библиотеки для работы с граф. объектами
using System.Windows.Forms; // подключение библиотеки для работы с компонентами Windows Forms
using System.IO; // подключение библиотеки ввода-вывода (в файлы)

namespace Архивация_BMP // пространство имен
{
    /// <summary>
    /// Главная (и единственная) форма приложения
    /// </summary>
    public partial class Frm : Form // описание класса формы
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Frm()
        {
            InitializeComponent(); // инициализировать все компоненты формы
            PB.Image = new Bitmap(PB.Width, PB.Height); //при запуске создадим картинку
        }

        /// <summary>
        /// Нажатие клавиши клавиатуры на форме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frm_KeyDown(object sender, KeyEventArgs e) 
        {   
            if (e.KeyCode == Keys.Escape) Close(); //выход при нажатии Escape
        }
        
        /// <summary>
        /// Обработчик нажатия клавиши BMP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBMP_Click(object sender, EventArgs e)
        {
            OD.InitialDirectory=Application.StartupPath; // путь нахождения ехе
            if (OD.ShowDialog() == DialogResult.OK) // если диалог выбора файла отработал
            {
                txtBMP.Text = OD.FileName; // сохранили имя выбранного файла
                txtBMP.SelectionStart = txtBMP.Text.Length; // курсор в конец строки
            }
        }
        
        /// <summary>
        /// Перевод часть байтов из массива в число int
        /// </summary>
        /// <param name="b">массив</param>
        /// <param name="bgn">индекс начального байта</param>
        /// <param name="end">индекс конечного байта</param>
        /// <returns></returns>
        int ByteToInt(byte[] b, int bgn, int end)
        {
            int x = 0; // результат
            for (int i = end; i >= bgn; i--) // от конечного к начальному
                x = (x << 8) + b[i]; // результат *256 и добавили байт
            return x; // результат функции - переменная x
        }
        
        /// <summary>
        /// Сравнение массивов по байтам
        /// </summary>
        /// <param name="a">первый массив</param>
        /// <param name="b">второй массив</param>
        /// <param name="szReal">длина сравнения</param>
        /// <returns></returns>
        bool Swap(byte[] a, byte[] b, int szReal)
        {
            for (int i = 0; i < szReal; i++) // проход по всем байтам (до указанной длины)
                if (a[i] > b[i]) return true; // если итерируемый байт первого массива больше второго - вернуть правду
                else // иначе
                    if (a[i] < b[i]) return false;// если итерируемый байт первого меньше больше второго - вернуть ложь
            return false; // если все байты равны - вернуть ложь
        }

        /// <summary>
        /// Перевод массива/строки под BWT алгоритм
        /// </summary>
        /// <param name="bt">входные байты</param>
        /// <param name="szReal">размер входного массива</param>
        /// <param name="ByteCount">количество байт в пикселе</param>
        /// <returns>номер строки для декодера</returns>
        int inBWT(ref byte[] bt, int szReal, int ByteCount)
        {
            int n = szReal / ByteCount; // количество пикселей в строке
            byte[][] a = new byte[n][]; // под квадратную матрицу n*n
            byte[] pix = new byte[ByteCount]; // массив под 1 пиксель
            for (int i = 0; i < n; i++) // по строкам матрицы
            {
                a[i] = new byte[szReal + 1]; // строка + байт флага исходной строки
                if (i == 0) // первая строка
                {
                    Array.Copy(bt, 0, a[0], 0, szReal); // скопировали исходную
                    a[0][szReal] = 1; // установили флаг
                }
                else // не первая строка
                {
                    Array.Copy(a[i - 1], 0, pix, 0, ByteCount); // первый пиксель из предыдущей строки
                    
                    //предыдущая копируентся в текущую с отступом на 1 пиксель
                    Array.Copy(a[i - 1], ByteCount, a[i], 0, szReal - ByteCount);

                    //пиксель в конец строки
                    Array.Copy(pix, 0, a[i], szReal - ByteCount, ByteCount);

                    a[i][szReal] = 0;//флаг обнулили
                }
            }

            //сортируем строки методом пузырька
            for (int i = n - 1; i > 0; i--)
            {
                bool swp = false;//флаг произошедшей замены
                for (int k = 0; k < i; k++) // вспомогательный цикл
                    if (Swap(a[k], a[k + 1], szReal)) // строки нужно менять
                    {
                        byte[] b = a[k]; // взаимно меняем
                        a[k] = a[k + 1]; // строки
                        a[k + 1] = b; // местами
                        swp = true; // флаг обмена
                    }
                if (!swp) break; // если не было замены, кончаем сортировку
            }
            
            int ns = 0; // номер исходной строки
            for (int i = 0; i < n; i++) // считываем/записываем последний столбик матрицы
            {
                Array.Copy(a[i], szReal - ByteCount, bt, i * ByteCount, ByteCount); // записали пиксель в bt
                if (a[i][szReal] == 1) ns = i; // нашли/запомнили номер исходной строки
                a[i] = null; // удалили строку
            }
            a = null; // удалили массив
            return ns; // вернули массив
        }

        /// <summary>
        /// Нажатие кнопки CODE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btCode_Click(object sender, EventArgs e)
        {
            lbRes.Text = ""; // устанавливаем текст надписи результата пустым
            lbRes.Refresh(); // обновляем надпись результата

            if (File.Exists(txtBMP.Text)) // если файл существует
                if (txtCode.Text != "") // есть куда кодировать
                {   
                    FileStream SR = File.Open(txtBMP.Text, FileMode.Open); // открыли ВМР на считывание
                    string name = Application.StartupPath + "\\" + txtCode.Text; // путь программы + имя результирующего файла
                    FileStream SW = File.Open(name, FileMode.Create); // создали файл на запись
                    byte[] bt = new byte[4]; // массив на считывание байт из файла
                    SR.Seek(10, SeekOrigin.Begin); // на 10й байт в файле
                    SR.Read(bt, 0, 4); // считали 4 байта
                    
                    //перевели в число начала изображения в файле,оно же размер заголовка файла
                    int Off = ByteToInt(bt, 0, 3);
                    
                    bt = new byte[Off]; // под заголовок
                    SR.Seek(0, SeekOrigin.Begin); // в начало файла
                    SR.Read(bt, 0, Off); // считали весь заголовок
                    SW.Write(bt, 0, Off); // записали весь заголовок
                    
                    int szF = ByteToInt(bt, 2, 7); // 2-7 байт - размер всего файла
                    int Wd = ByteToInt(bt, 18, 21); // 18-21 байт - ширина рисунка в пикселях
                    int Hg = ByteToInt(bt, 22, 25); // 22-25 байт - количество строк в рисунке
                    
                    // 28-29 байт количество бит на пиксель, поделенное на 8
                    int ByteCount = ByteToInt(bt, 28, 29) / 8; // количество байт в пикселе
                    
                    int szNew = Off; // под размер нового файла
                    int szReal = Wd * ByteCount; // количество байт на строку
                    int szLine = szReal / 4 * 4; // определяем сколько байт на строку в файле
                    if (szReal % 4 > 0) szLine += 4; // должно быть кратно 4
                    bt = new byte[szLine]; // массив под строку
                    SR.Seek(Off, SeekOrigin.Begin); // встали на начало строк в ВМР файле
                    for (int i = 0; i < Hg; i++) // пошли по строкам
                    {
                        SR.Read(bt, 0, szLine); // прочитали строку
                        if (cbBWT.Checked) // ипользуем алгоритм BWT
                        {
                            // строку подвергли BWT и знаем ее номер для декодировки
                            int n = inBWT(ref bt, szReal, ByteCount);
                            SW.WriteByte((byte)(n % 256)); // записали номер
                            SW.WriteByte((byte)((n / 256) & 0xFF)); // побайтно 2 байта
                        }

                        //строку отправили на RLE,получили закодированный список байт
                        List<byte> lB = Code(bt, szReal, ByteCount);

                        WriteList(lB, ref SW); // записали список в файл

                        //увеличили размер на колтчество записанного
                        szNew += (cbBWT.Checked ? 2 : 0) + lB.Count;
                    }

                    SW.Close(); // закрыли 
                    SR.Close(); // оба файла
                    
                    string str = "Создан файл \"" + txtCode.Text + "\","; // строка сообщения
                    str += string.Format("cжатие до {0:F2}%", szNew / (double)szF * 100); // рассчитали коэффицент сжатия
                    lbRes.Text = str; // вывели сообщение
                    Decode(); // декодировали файл
                }
                else // если файл BMP-кодирования не существует
                    MessageBox.Show("Файл кодирования отсутсвует!"); // выводим соответствующее сообщения
            else // если основной файл не существует
                MessageBox.Show("Файл ВМР отсутсвует!"); // выводим соответствующее сообщения
        }

        /// <summary>
        /// Структура для декодировки BWT
        /// </summary>
        struct BWT
        {
            public byte[] bt; // считанные байты
            public int a, b; // номер строки в исходной и отсортированной позициях
        }

        /// <summary>
        /// Декодировка из BWT
        /// </summary>
        /// <param name="str">строка для декодирования</param>
        /// <param name="nStr">номер строки оригинальной</param>
        /// <param name="ByteCount">количество байт в пикселе</param>
        void outBWT(ref byte[] str, int nStr, int ByteCount)
        {
            int n = str.Length / ByteCount; // количество пикселей 
            BWT[] a = new BWT[n]; // n позиций ля декодировки

            for (int i = 0; i < n; i++) // проход по всем пикселями
            {
                a[i].bt = new byte[ByteCount]; // выделили массив под количество байт в пикселе
                Array.Copy(str, i * ByteCount, a[i].bt, 0, ByteCount); // записали прочитанный пиксель
                a[i].b = i; // номер позиции
            }

            for (int i = n - 1; i > 0; i--) // сортировка пузырьком
            {
                bool swp = false; // метка для сортировки
                for (int k = 0; k < i; k++) // вспомогательный цикл
                    if (Swap(a[k].bt, a[k + 1].bt, ByteCount)) // если необходимо обмен
                    { 
                        BWT x = a[k]; // меняем
                        a[k] = a[k + 1]; // оба
                        a[k + 1] = x; // попарных
                        swp = true; // пиксела и ставим отметку
                    }
                if (!swp) break; // если отметка не была поставлена - выходим из цикла
            }

            for (int i = 0; i < n; i++) // по всем пикслеям
                for (int k = 0; k < n; k++) // вспомогателньый цикл
                    if (a[k].b == i) // если обозревая строка - совпадает с номером итератора 
                    {
                        a[i].a = k; // записали этот номер
                        break; // вышли из цикла
                    }

            int f = nStr; // запомнили номер оригинальной строки

            for (int i = 0; i < n; i++) // проход по всем пикселями
            {
                if (i == 0) // если это первая строка
                    Array.Copy(a[f].bt, 0, str, 0, ByteCount); // записываем байты как есть
                else
                    Array.Copy(a[f].bt, 0, str, (n - i) * ByteCount, ByteCount); // записываем байты с условием смещения
                f = a[f].a; // запоминаем текущий номер обрабатываемой строки
            }
        }

        /// <summary>
        /// Декодировка из RLE
        /// </summary>
        void Decode()
        {
            string name = Application.StartupPath + "\\" + txtCode.Text; // получаем имя закодированного файла
            FileStream SR = File.Open(name, FileMode.Open); // открываем файл по полученному имени
            name += ".bmp"; // указываем расширение декодированного файла

            FileStream SW = File.Open(name, FileMode.Create); // открыаем выходной поток
            byte[] bt = new byte[4]; // подготовленный массив байт

            SR.Seek(10, SeekOrigin.Begin); // сдвигаемся на 10 байт вперед в закодированном файле
            SR.Read(bt, 0, bt.Length); // прочитали 4 байта

            int Off = ByteToInt(bt, 0, bt.Length - 1); // высчитали размер заголовка в закодированном файле
            bt = new byte[Off]; // массив под размер заголовка

            SR.Seek(0, SeekOrigin.Begin); // сдинулись в начало
            SR.Read(bt, 0, bt.Length); // прочитали весь заголовок будущего BMP-файла
            SW.Write(bt, 0, Off); // записали в выходной поток

            int Wd = ByteToInt(bt, 18, 23); // размер линии картинки в точках
            int ByteCount = ByteToInt(bt, 28, 29) / 8; // количество байт в пикселе
            int szReal = Wd * ByteCount; // количество всех байт в линии
            int szLine = szReal / 4 * 4; // размер (в байтах) линии в картинке

            if (szReal % 4 > 0) szLine += 4; // добавляем выравнивание

            int dSz = szLine - szReal; // размер необходимого выравнивания
            bt = new byte[128 * ByteCount]; // выделение массива под байты

            byte[] str = new byte[szReal]; // выходной массив 
            byte[] pix = new byte[ByteCount]; // один пиксель
            byte[] bStr = new byte[2]; // два байта
            int n, nStr, nWrite; // длина последовательности, номер строки, количество записанных байт

            nStr = nWrite = 0; // обнуляем номер строк (для BWT) и количество записанных байт

            while (SR.Position < SR.Length) // пока не прочитан весь входной файл
            {
                if (nWrite == 0 && cbBWT.Checked) // если мы в начале файле и стоит отметка о BWT
                {
                    SR.Read(bStr, 0, 2); // прочитали два первых байта
                    nStr = ByteToInt(bStr, 0, 1); // швысчитали номер оригинальной строки (см. алгоритм BWT)
                }

                byte b = (byte)SR.ReadByte(); // записали считанный байт

                if ((b & 0x80) == 0) // Если считанный байт – флаг начала последовательности повторяющихся пикселей
                {
                    n = (b & 0x7F) + 2; // узнаем длину последовательности из флага
                    SR.Read(pix, 0, ByteCount); // прочитали пиксель

                    // записываем в выходной массив данный пиксель столько раз подряд,
                    // сколько равняется длина высчитанной последовательности
                    for (int i = 0; i < n; i++) // для последовательности длины n
                        if (cbBWT.Checked) // если включено условие BWT
                            Array.Copy(pix, 0, str, nWrite + i * ByteCount, ByteCount); // пишем с условием смещения по BWT
                        else // без условия BWT
                            SW.Write(pix, 0, ByteCount); // сразу пишем пиксель
                }
                else // Если считанный байт – флаг начала последовательности неповторяющихся пикселей
                {
                    n = (b & 0x7F) + 1; // узнаем длину последовательности из флага
                    SR.Read(bt, 0, n * ByteCount); // прочитали пиксель

                    if (cbBWT.Checked) // если включено условие BWT
                        Array.Copy(bt, 0, str, nWrite, n * ByteCount); // пишем с условием смещения по BWT
                    else // без условия BWT
                        SW.Write(bt, 0, n * ByteCount); // сразу пишем пиксель
                }

                nWrite += n * ByteCount; // запоминаем количество записанных пикселей

                if (nWrite == szReal) // если дошли до конца
                {
                    if (cbBWT.Checked)  // если включено условие BWT
                    {
                        outBWT(ref str, nStr, ByteCount); // используем декодер BWT
                        SW.Write(str, 0, szReal); // записали в выходной поток декодированную последовательность байт
                    }

                    if (dSz > 0) SW.Write(bt, 0, dSz); // если нужно записать также и выравнивающие байты - запишем их
                    nWrite = 0; // обнулим кол-во считанных байт
                }
            }

            SW.Close(); // закрываем выходной поток
            SR.Close(); // закрываем входной поток

            using (Bitmap bmp = new Bitmap(name)) // создаем новую bmp-картинку
            {
                using (Graphics g = Graphics.FromImage(PB.Image)) // используем графику на ней
                {
                    g.Clear(Color.Black); // зальем картинку черным цветом

                    if (PB.Width < bmp.Width || PB.Height < bmp.Height) // если влезает в pictureBox на форме
                    {
                        float cfW = (float)PB.Width / bmp.Width; // узнаем коэффициент нового размера по ширине
                        float cfH = (float)PB.Height / bmp.Height; // узнаем коэффициент нового размера по высоте
                        float cf = cfH < cfW ? cfH : cfW; // узнаем результирующий коэффициент

                        g.DrawImage(bmp, 0, 0, bmp.Width * cf, bmp.Height * cf); // отображаем картинку
                    }
                    else // не влезает в pictureBox на форме
                        g.DrawImage(bmp, 0, 0); // записываем как есть
                    PB.Refresh(); // обновить pictureBox на форме
                }
            }
            File.Delete(name); // удалить закодированный файл
        }

        /// <summary>
        /// Записывает лист байтов в файл
        /// </summary>
        /// <param name="lB">входная коллекция байт</param>
        /// <param name="SW">выходной поток</param>
        void WriteList(List<byte> lB, ref FileStream SW)
        {
            foreach (byte b in lB) // для каждого байта
                SW.WriteByte(b); // записать в выходной поток
        }

        /// <summary>
        /// Сравнивает массивы/пиксель на содержимое
        /// </summary>
        /// <param name="a">первый массив</param>
        /// <param name="b">второй массив</param>
        /// <returns>результат сравнения</returns>
        bool Equal(byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++) // проходимся по всем байтам первого массива
                if (a[i] != b[i]) return false; // если пиксели одинаковы - вернуть ложь
            return true; // массивы не одинаковы
        }

        /// <summary>
        /// Кодировка строки файла в список байтов алгоритмом RLE
        /// </summary>
        /// <param name="bt">входной массив байт</param>
        /// <param name="szReal">размер для кодирования</param>
        /// <param name="ByteCount">количество байт в пикселе</param>
        /// <returns>коллекция кодированных байт</returns>
        List<byte> Code(byte[] bt, int szReal, int ByteCount)
        {
            int nDup, nSol; // количество в сериях повторов и неповторяющихся
            nDup = nSol = 0; // пока же ничего, они же и будут флагами что серия начата

            //список используем т.к. заранее не знаем размер результата,
            //он может быть и больше исходного
            List<byte> lB = new List<byte>(); // лист/список результата кодирования
            byte[] pix0 = new byte[ByteCount]; // под текущий пиксель
            byte[] pix1 = new byte[ByteCount]; // под следующий пиксель
            byte[] pDup = new byte[ByteCount]; // пиксель повтора

            for (int i = 0; i < szReal; i += ByteCount) // пошли по строке
            {
                if (nSol == 128) // если серия одиночных и ее длина достигла предела  
                {   
                    int idx = lB.Count - 128 * ByteCount - 1; //индекс в листе под количество в серии
                    lB[idx] = 0xFF; // бит флага установлен и 127 что=128
                    nSol = 0; // сбросили серию
                }
                Array.Copy(bt, i, pix0, 0, ByteCount); // скопировали текущий пиксель
                if (i < szReal - ByteCount) // если не в конце строки
                    Array.Copy(bt, i + ByteCount, pix1, 0, ByteCount); // скопировали следующий пиксель

                //если серия повторов достигла 129,или стоим на краю строки,
                //или следущий пиксель не равен текущему
                if (nDup == 129 || (nDup > 0 && (i == szReal - ByteCount || !Equal(pix0, pix1)))) 
                {
                    lB.Add((byte)(nDup - 2)); // в лист количество серии повторов(-2)
                    for (int k = 0; k < ByteCount; k++) // в лист пиксель повторов 
                        lB.Add(pDup[k]); // добавляем в лист
                    nDup = 0; // сбросили серию
                    continue; // к следующему пикселю
                }
                
                if (nSol > 0) // серия одиночных запущена
                    if (i == szReal - ByteCount) // последний пиксель в строке
                    {
                        for (int k = 0; k < ByteCount; k++) // проход по всем байтам в пикселе
                            lB.Add(pix0[k]); // в лист текущий пиксель

                        nSol++;//реальное количество
                        
                        int idx = lB.Count - nSol * ByteCount - 1; //индекс в листе под количество в серии

                        //0х80=10000000 старший бит установлен,т.е. серия несовпадающих
                        lB[idx] = (byte)(0x80 + --nSol);//количество-1 с флагом
                    }
                    else // не последний пиксель в строке
                        // текущий и следующий пиксели совпадают,закансиваем серию одиночных
                        if (Equal(pix0, pix1))
                        {   
                            int idx = lB.Count - nSol * ByteCount - 1; //индекс в листе под количество в серии

                            //0х80=10000000 старший бит установлен,т.е. серия несовпадающих
                            lB[idx] = (byte)(0x80 + --nSol); // количество-1 с флагом

                            nSol = 0; // сбросили серию
                            nDup = 2; // запустили серию повторов

                            Array.Copy(bt, i, pDup, 0, ByteCount); // сохранили пиксель повторов
                        }
                        else // серия одиночных продолжается
                        {
                            for (int k = 0; k < ByteCount; k++) // проход по всем байтам в пикселе
                                lB.Add(pix0[k]); // в лист текущий пиксель
                            nSol++; // увеличили количество
                        }
                else // не запущена серия одиночных
                    // если серия повторов запущена 
                    if (nDup>0) nDup++; // увеличили количество
                    else // ни одна из серий не запущена
                        //если на краю строки или пиксели не совпадают
                        if (i == szReal - ByteCount || !Equal(pix0, pix1))
                        {   //запустим одиночную серию
                            nSol = 1; // количество

                            //0х80=10000000 старший бит установлен,т.е. серия несовпадающих
                            lB.Add(0x80); // в лист под количество
                            for (int k = 0; k < ByteCount; k++) // проход по всем байтам в пикселе
                                lB.Add(pix0[k]);//в лист текущий пиксель
                        }
                        else // пиксели совпали
                        {
                            nDup = 2; // серия дублей
                            Array.Copy(bt, i, pDup, 0, ByteCount); // сохранили пиксель повторов
                        }
            }
            return lB; // вернули закодированный лист байтов
        }

        private void Frm_Load(object sender, EventArgs e)
        {

        }
    }
}

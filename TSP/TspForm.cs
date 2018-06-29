using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Linq;

namespace коммивояжер
{
    public partial class TspForm : Form
    {
        Cities cityList = new Cities();

        PictureBox picture = new PictureBox();
        RadioButton radiobutton1 = new RadioButton();
        RadioButton radiobutton2 = new RadioButton();
        RadioButton radiobutton3 = new RadioButton();
        RadioButton radiobutton4 = new RadioButton();
        Label label = new Label();
        Label statusLabel = new Label();
        Label lastIterationLabel = new Label();//
        Label lastTourLengthLabel = new Label();//
        Label timeLabel = new Label();//
        Label numberCitiesLabel = new Label();//
        Label lastIterationValue = new Label();//
        Label lastTourLengthValue = new Label();//
        Label timeValue = new Label();//
        Label numberCitiesValue = new Label();//
        TextBox textBox = new TextBox();
        Button button = new Button();
        Button button1 = new Button();
        Button openCityListButton = new Button();
        Button clearCityListButton = new Button();
        Button findButton = new Button();
        Panel panel = new Panel();

        Image cityImage;
        Graphics graphics;


        string tempS1 = "";
        string tempS2 = "";
        int[,] mas;
        int[] minm;
        int[] m;
        int towns;
        int min;
        int s;
        int count;
        bool found = true;

        System.Diagnostics.Stopwatch myStopwatch;



        public TspForm()
        {
            InitializeComponent();
            this.Size = new Size(800, 650);
            this.Text = "Метод ветвей и границ";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScroll = false;

            picture.Size = new Size(500, 500);
            picture.Location = new Point(15, 15);
            picture.BackColor = Color.White;
            this.Controls.Add(picture);

            lastTourLengthLabel.Location = new Point(15, picture.Location.Y + picture.Size.Height + 10);/////
            lastTourLengthLabel.Text = "Минимальный путь: ";
            lastTourLengthLabel.AutoSize = true;
            this.Controls.Add(lastTourLengthLabel);

            lastIterationLabel.Location = new Point(15, lastTourLengthLabel.Location.Y + lastTourLengthLabel.Size.Height + 10);///////
            lastIterationLabel.Text = "Порядок переездов: ";
            lastIterationLabel.AutoSize = true;
            this.Controls.Add(lastIterationLabel);

            timeLabel.Location = new Point(15, lastIterationLabel.Location.Y + lastIterationLabel.Size.Height + 10);///////
            timeLabel.Text = "Общее время: ";
            timeLabel.AutoSize = true;
            this.Controls.Add(timeLabel);

            numberCitiesLabel.Location = new Point(15, timeLabel.Location.Y + timeLabel.Size.Height + 10);
            numberCitiesLabel.Text = "Количество городов: ";
            numberCitiesLabel.AutoSize = true;
            this.Controls.Add(numberCitiesLabel);


            lastTourLengthValue.Location = new Point(15 + lastTourLengthLabel.Size.Width + 10, picture.Location.Y + picture.Size.Height + 10);//////
            lastTourLengthValue.AutoSize = true;
            this.Controls.Add(lastTourLengthValue);

            lastIterationValue.Location = new Point(15 + lastIterationLabel.Size.Width + 10, lastTourLengthLabel.Location.Y + lastTourLengthLabel.Size.Height + 10);/////////
            lastIterationValue.AutoSize = true;
            this.Controls.Add(lastIterationValue);

            timeValue.Location = new Point(15 + timeLabel.Size.Width + 10, lastIterationLabel.Location.Y + lastIterationLabel.Size.Height + 10);////
            timeValue.AutoSize = true;
            this.Controls.Add(timeValue);

            numberCitiesValue.Location = new Point(15 + numberCitiesLabel.Size.Width + 10, timeLabel.Location.Y + timeLabel.Size.Height + 10);/////
            numberCitiesValue.AutoSize = true;
            this.Controls.Add(numberCitiesValue);

            radiobutton1.Location = new Point(550, 50);
            radiobutton1.Text = "Ввод XML файла";
            radiobutton1.Font = new System.Drawing.Font(radiobutton1.Font.Name, 10f);
            radiobutton1.AutoSize = true;
            radiobutton1.Checked = true;
            this.Controls.Add(radiobutton1);

            radiobutton2.Location = new Point(550, 80);
            radiobutton2.Text = "Ввод случайных городов";
            radiobutton2.Font = new System.Drawing.Font(radiobutton2.Font.Name, 10f);
            radiobutton2.AutoSize = true;
            radiobutton2.Checked = false;
            this.Controls.Add(radiobutton2);

            radiobutton3.Location = new Point(550, 110);
            radiobutton3.Text = "Ввод матрицы";
            radiobutton3.Font = new System.Drawing.Font(radiobutton3.Font.Name, 10f);
            radiobutton3.AutoSize = true;
            radiobutton3.Checked = false;
            this.Controls.Add(radiobutton3);

            radiobutton4.Location = new Point(550, 140);
            radiobutton4.Text = "Ввод вручную";
            radiobutton4.Font = new System.Drawing.Font(radiobutton4.Font.Name, 10f);
            radiobutton4.AutoSize = true;
            radiobutton4.Checked = false;
            this.Controls.Add(radiobutton4);

            panel.Size = new Size(210, 200);
            panel.Location = new Point(540, 170);
            panel.BackColor = Color.White;
            this.Controls.Add(panel);

            clearCityListButton.Location = new Point(550, 380);
            clearCityListButton.Text = "Clear City List";
            clearCityListButton.Font = new System.Drawing.Font(clearCityListButton.Font.Name, 10f);
            clearCityListButton.AutoSize = true;
            this.Controls.Add(clearCityListButton);

            findButton.Location = new Point(550, 420);
            findButton.Text = "Start";
            findButton.Font = new System.Drawing.Font(findButton.Font.Name, 10f);
            findButton.Size = new Size(100, 40);
            this.Controls.Add(findButton);

            statusLabel.Location = new Point(10, 550);
            statusLabel.Text = "";
            statusLabel.Font = new System.Drawing.Font(statusLabel.Font.Name, 10f);
            statusLabel.AutoSize = true;
            this.Controls.Add(statusLabel);

            //events
            if (radiobutton1.Checked)
                radiobutton1_CheckedChanged(null, new EventArgs());
            radiobutton1.CheckedChanged += new EventHandler(radiobutton1_CheckedChanged);
            radiobutton2.CheckedChanged += new EventHandler(radiobutton2_CheckedChanged);
            radiobutton3.CheckedChanged += new EventHandler(radiobutton3_CheckedChanged);
            radiobutton4.CheckedChanged += new EventHandler(radiobutton4_CheckedChanged);
            clearCityListButton.Click += new EventHandler(clearCityListButton_Click);
            findButton.Click += new EventHandler(findButton_Click);


        }
        void radiobutton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobutton1.Checked)
            {
                panel.Controls.Clear();
                clearCityListButton_Click(null, new EventArgs());
                
                label.Location = new Point(10, 10);
                label.Text = "City XML File Name";
                label.Font = new System.Drawing.Font(label.Font.Name, 10f);
                label.AutoSize = true;
                panel.Controls.Add(label);

                textBox.Location = new Point(10, 40);
                textBox.Text = "../../Cities.xml";
                textBox.Font = new System.Drawing.Font(textBox.Font.Name, 10f);
                textBox.Size = new Size(150, 25);
                panel.Controls.Add(textBox);

                button.Location = new Point(10,70);
                button.Text = "Browse";
                button.Font = new System.Drawing.Font(button.Font.Name, 10f);
                button.AutoSize = true;
                panel.Controls.Add(button);

                openCityListButton.Location = new Point(10, 105);
                openCityListButton.Text = "Open city";
                openCityListButton.Font = new System.Drawing.Font(openCityListButton.Font.Name, 10f);
                openCityListButton.AutoSize = true;
                panel.Controls.Add(openCityListButton);

                //events
                button.Click += new EventHandler(selectFileButton_Click);
                openCityListButton.Click += new EventHandler(openCityListButton_Click);
            }
            else
            {
                button.Click -= new EventHandler(selectFileButton_Click);
                openCityListButton.Click -= new EventHandler(openCityListButton_Click);
            }
        }
        void radiobutton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobutton2.Checked)
            {
                panel.Controls.Clear();
                clearCityListButton_Click(null, new EventArgs());

                label.Location = new Point(10, 10);
                label.Text = "Change Cities Amount";
                label.Font = new System.Drawing.Font(label.Font.Name, 10f);
                label.AutoSize = true;
                panel.Controls.Add(label);

                textBox.Location = new Point(10, 40);
                textBox.Text = "5";
                textBox.Font = new System.Drawing.Font(textBox.Font.Name, 10f);
                textBox.Size = new Size(150, 25);
                panel.Controls.Add(textBox);

                button.Location = new Point(10, 70);
                button.Text = "Create Cities";
                button.Font = new System.Drawing.Font(button.Font.Name, 10f);
                button.AutoSize = true;
                panel.Controls.Add(button);


                button.Click += new EventHandler(ButtonArray_Click);
            }
            else
            {
                button.Click -= new EventHandler(ButtonArray_Click);
            }
        }
        void radiobutton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobutton3.Checked)
            {
                panel.Controls.Clear();
                clearCityListButton_Click(null, new EventArgs());

                label.Location = new Point(10, 10);
                label.Text = "Array TXT File Name";
                label.Font = new System.Drawing.Font(label.Font.Name, 10f);
                label.AutoSize = true;
                panel.Controls.Add(label);

                textBox.Location = new Point(10, 40);
                textBox.Text = "../../input.txt";
                textBox.Font = new System.Drawing.Font(textBox.Font.Name, 10f);
                textBox.Size = new Size(150, 25);
                panel.Controls.Add(textBox);

                button.Location = new Point(10, 70);
                button.Text = "Browse";
                button.Font = new System.Drawing.Font(button.Font.Name, 10f);
                button.AutoSize = true;
                panel.Controls.Add(button);

                button1.Location = new Point(10, 105);
                button1.Text = "Open array";
                button1.Font = new System.Drawing.Font(openCityListButton.Font.Name, 10f);
                button1.AutoSize = true;
                panel.Controls.Add(button1);

                //events
                button.Click += new EventHandler(selectFileButton_Click);
                button1.Click += new EventHandler(ButtonArray_Click);
            }
            else
            {
                button.Click -= new EventHandler(selectFileButton_Click);
                button1.Click -= new EventHandler(ButtonArray_Click);
            }
        }
        void radiobutton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radiobutton4.Checked)
            {
                panel.Controls.Clear();
                clearCityListButton_Click(null, new EventArgs());

                picture.MouseDown += new MouseEventHandler(picture_MouseDown);
            }
            else
            {
                picture.MouseDown -= new MouseEventHandler(picture_MouseDown);
            }
        }

        /// <summary>
        /// User clicked a point on the city map.
        /// As long as we aren't running the TSP algorithm,
        /// place a new city on the map and add it to the city list.
        /// </summary>
        /// <param name="sender">Object that generated this event.</param>
        /// <param name="e">Event arguments.</param>
        private void picture_MouseDown(object sender, MouseEventArgs e)
        {
            /*if (tsp != null)
            {
                statusLabel.Text = "Cannot alter city list while running";
                statusLabel.ForeColor = Color.Red;
                return;
            }*/

            cityList.Add(new City(e.X, e.Y));
            DrawCityList(cityList);
        }

        /// <summary>
        /// Draw just the list of cities.
        /// </summary>
        /// <param name="cityList">The list of cities to draw.</param>
        private void DrawCityList(Cities cityList = null, bool flag = false)
        {
            cityImage = new Bitmap(picture.Width, picture.Height);
            graphics = Graphics.FromImage(cityImage);

            int temp = 0;
            if (!flag)
                temp = cityList.Count;
            else
                temp = cityList.Count / 2;

            numberCitiesValue.Text = temp.ToString();
            string drawString;
            for (int i = 0; i < temp; i++)
            {
                // Draw a circle for the city.                
                graphics.DrawEllipse(Pens.Black, cityList[i].Location.X - 2, cityList[i].Location.Y - 2, 12, 12);

                drawString = (i + 1).ToString();
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 7, FontStyle.Regular);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                graphics.DrawString(drawString, drawFont, drawBrush, cityList[i].Location.X - 18, cityList[i].Location.Y + 5, drawFormat);
            }

                this.picture.Image = cityImage;

        }

        private void ButtonArray_Click(object sender, EventArgs e)
        {
            if (radiobutton2.Checked || radiobutton3.Checked)
            {
                Input();

                DrawCityList(cityList);
            }
        }

        public void DrawLine(int lastCity, int nextCity)
        {
            Pen p = new Pen(Color.Black);
            p.StartCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
            p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            Brush b = new SolidBrush(p.Color);
            // Draw the line connecting the city.
            graphics.DrawLine(p, cityList[nextCity - 1].Location, cityList[lastCity - 1].Location);

            float x1 = cityList[nextCity - 1].Location.X;
            float x2 = cityList[lastCity - 1].Location.X;
            float y1 = cityList[nextCity - 1].Location.Y;
            float y2 = cityList[lastCity - 1].Location.Y;

            double angle = Math.Atan2(y2 - y1, x2 - x1);
            PointF p1 = new PointF(x1 + (float)(Math.Cos(angle - Math.PI / 2.0) * 3), y1 + (float)(Math.Sin(angle - Math.PI / 2.0) * 3));
            PointF p2 = new PointF(x1 - (float)(Math.Cos(angle) * 10), y1 - (float)(Math.Sin(angle) * 10));
            PointF p3 = new PointF(x1 - (float)(Math.Cos(angle - Math.PI / 2.0) * 3), y1 - (float)(Math.Sin(angle - Math.PI / 2.0) * 3));

            graphics.FillPolygon(b, new PointF[] { p1, p2, p3 });
            picture.Refresh();
        }

        /// <summary>
        /// User has chosen to open the 
        /// Not allowed if running the TSP algorithm.
        /// </summary>
        /// <param name="sender">Object that generated this event.</param>
        /// <param name="e">Event arguments.</param>
        private void openCityListButton_Click(object sender, EventArgs e)
        {
            string fileName = "";

            try
            {
                fileName = this.textBox.Text;

                cityList.OpenCityList(fileName);
                DrawCityList(cityList);
                Input();
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("File not found: " + fileName, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Cities XML file is not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        /// <summary>
        /// User is selecting a new city list XML file.
        /// Not allowed if running the TSP algorithm.
        /// </summary>
        /// <param name="sender">Object that generated this event.</param>
        /// <param name="e">Event arguments.</param>
        private void selectFileButton_Click(object sender, EventArgs e)
        {
            if (radiobutton1.Checked)
            {
                OpenFileDialog fileOpenDialog = new OpenFileDialog();
                fileOpenDialog.Filter = "XML(*.xml)|*.xml";
                fileOpenDialog.InitialDirectory = Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString()).ToString();
                fileOpenDialog.ShowDialog();
                textBox.Text = fileOpenDialog.FileName;
            }
            else if (radiobutton3.Checked)
            {
                OpenFileDialog fileOpenDialog = new OpenFileDialog();
                fileOpenDialog.Filter = "TXT(*.txt)|*.txt";
                fileOpenDialog.InitialDirectory = Directory.GetParent(Directory.GetParent(Application.StartupPath).ToString()).ToString();
                fileOpenDialog.ShowDialog();
                textBox.Text = fileOpenDialog.FileName;
            }
        }

        /// <summary>
        /// User has selected to clear the city list.
        /// Not allowed if running the TSP algorithm.
        /// </summary>
        /// <param name="sender">Object that generated this event.</param>
        /// <param name="e">Event arguments.</param>
        private void clearCityListButton_Click(object sender, EventArgs e)
        {
            numberCitiesValue.Text = "";
            cityList.Clear();
            this.DrawCityList(cityList);
        }

        /// <summary>
        /// User clicked the start button to start the TSP algorithm.
        /// If it is already running, then this button will say stop and we will stop the TSP.
        /// Otherwise, 
        /// </summary>
        /// <param name="sender">Object that generated this event.</param>
        /// <param name="e">Event arguments.</param>
        private void findButton_Click(object sender, EventArgs e)
        {
            if (findButton.Text == "Stop")
            {
                return;
            }
            else if (cityList.Count < 3)
            {
                MessageBox.Show("You must to place on map at least 3 cities. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            else
            {
                this.findButton.Text = "Stop";
                BeginTsp();
            }
        }


        /// <summary>
        /// Starts up the TSP class.
        /// This function executes on a thread pool thread.
        /// </summary>
        /// <param name="stateInfo">Not used</param>
        private void BeginTsp()
        {
            Input();//инициализация
            myStopwatch = new System.Diagnostics.Stopwatch();//таймер
            myStopwatch.Start(); //запуск

            Run();
            Output();

            myStopwatch.Stop();

            lastTourLengthValue.Text = tempS1;
            lastIterationValue.Text = tempS2;
            timeValue.Text = myStopwatch.Elapsed.ToString();
            

        }
        
        private void Input()
        {
            if(radiobutton1.Checked)
            {
                cityList.CalculateCityDistances();
                mas = new int[cityList.Count,cityList.Count];
                towns = cityList.Count;

                m = new int[towns];
                minm = new int[towns];

                for (int i = 0; i < cityList.Count; i++)
                {
                    for (int j = 0; j < cityList.Count; j++)
                    {
                        mas[i, j] = (int)cityList[i].Distances[j];
                        if (i == j)
                        {
                            mas[i, j] = 0;
                        }
                    }
                }
            }
            else if(radiobutton2.Checked)
            {
                towns = Convert.ToInt32(textBox.Text);
                mas = new int[towns, towns];

                m = new int[towns];
                minm = new int[towns];

                Random rand = new Random();
                for (int i = 0; i < towns; i++)
                {
                    for (int j = 0; j < towns; j++)
                    {
                        mas[i, j] = rand.Next(1, 100);                //считали матрицу расстояний
                        if (i == j)
                            mas[i, j] = 0;
                    }
                    City city = new City(rand.Next(1, 400) + 50, rand.Next(1, 400) + 50);
                    cityList.Add(city);
                }
            }
            else if(radiobutton3.Checked)
            {
                System.IO.FileStream fs = new System.IO.FileStream(textBox.Text, System.IO.FileMode.Open);

                using (System.IO.StreamReader reader = new System.IO.StreamReader(fs))
                {
                    // Get the town count
                    string currentLine = reader.ReadLine();
                    string extractedLine = currentLine.Substring(0);
                    towns = int.Parse(extractedLine);
                    mas = new int[towns, towns];

                    m = new int[towns];
                    minm = new int[towns];

                    for (int i = 0; i < towns; i++)
                    {
                        char[] ch = new char[towns];
                        currentLine = reader.ReadLine();
                        if (currentLine != null)
                        {
                            extractedLine = currentLine.Substring(0);
                            string[] str = extractedLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int j = 0; j < towns; j++)
                            {
                                mas[i, j] = Convert.ToInt32(str[j]);                //считали матрицу расстояний
                                if (i == j)
                                    mas[i, j] = 0;
                            }
                        }
                    }
                    Random rand = new Random();
                    for (int i = 0; i < towns; i++)
                    {
                        City city = new City(rand.Next(1, 400) + 50, rand.Next(1, 400) + 50);
                        cityList.Add(city);
                    }
                }
            }
            else if(radiobutton4.Checked)
            {
                cityList.CalculateCityDistances();
                towns = cityList.Count;
                mas = new int[towns, towns];

                m = new int[towns];
                minm = new int[towns];

                for (int i = 0; i < towns; i++)
                {
                    for (int j = 0; j < towns; j++)
                    {
                        mas[i, j] = (int)cityList[i].Distances[j];
                        if (i == j)
                        {
                            mas[i, j] = 0;
                        }
                    }
                }
            }
        }


        void Output()                    //вывод данных
        {
            if (found)                        //если найден маршрут...
            {
                tempS1 = min.ToString();

                int c = 1;                    //номер в порядке обхода городов
                int[] temparr = new int[towns];
                for (int i = 0; i < towns; i++)      //пробегаем по всем городам
                {
                    int j = 0;
                    while ((j < towns) &&                //ищем следующий город в порядке обхода    
                    (minm[j] != c))
                    {
                        j++;
                    }
                    
                    tempS2 += (j + 1).ToString() + "->";
                    c++;
                    temparr[i] = j + 1;
                }
                tempS2 += (minm[1] + 1).ToString();
                for (int i = 1; i < towns; i++)
                {
                    DrawLine(temparr[i - 1], temparr[i]);
                }
                DrawLine(temparr[towns - 1], temparr[0]);
            }
        }

        void Search(int x)                //поиск следующего города в порядке обхода после города с номером Х
        {
            if ((count == towns) &&                //если просмотрели все города
            (mas[x, 1] != 0) &&                //из последнего города есть путь в первый город
            (s + mas[x, 1] < min))            //новая сумма расстояний меньше минимальной суммы
            {
                found = true;                    //маршрут найден
                min = s + mas[x, 1];                //изменяем: новая минимальная сумма расстояний
                for (int i = 0; i < towns; i++)
                {
                    minm[i] = m[i];//изменяем: новый минимальный путь
                }
            }
            else
            {
                for (int i = 0; i < towns; i++)     //из текущего города просматриваем все города
                {
                    if ((i != x) &&                //новый город не совпадает с текущим    
                    (mas[x, i] != 0) &&            //есть прямой путь из x в i
                    (m[i] == 0) &&            //новый город еще не простотрен
                    (s + mas[x, i] < min))    //текущая сумма не превышает минимальной
                    {
                        s += mas[x, i];                //наращиваем сумму
                        count++;                //количество просмотренных городав
                        m[i] = count;                //отмечаем у нового города новый номер в порядке обхода

                        Search(i);                //поиск нового города начиная с города i
                        m[i] = 0;                    //возвращаем все назад
                        count--;                //-"-
                        s -= mas[x, i];                //-"-
                    }
                }
            }
        }
        void Run()
        {                                //инициализация                                
            s = 0;
            found = false;
            min = 32767;
            for (int i = 0; i < towns; i++)
            {
                m[i] = 0;
            }
            count = 1;
            m[1] = count;                        //считаем что поиск начинается с первого города
            Search(1);                        //считаем что поиск начинается с первого города
        }
    }
}

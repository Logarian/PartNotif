using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace PartNotif
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SqlCommand cmd, cmd2, cmd3, cmd4;
        private SqlConnection con;
        private string _c = @"Data Source=ServerName;Initial Catalog=DataBaseName;Integrated Security=True";

        private void Form1_Load(object sender, EventArgs e)
        {
            /*Выпадающий список БД*/
            con = new SqlConnection(_c);
            con.Open();
            cmd = new SqlCommand(@"select
                                    name
                                    ,substring(name, 6,3)
                                    ,substring(name, 14,2)
                                    from
                                    sys.databases
                                    where substring(name, 6,3) = 'gia'
                                    and substring(name, 14,2) >= 19 
                                    and name not like ('%_app')
                                    and name not like ('%_dt')", con);
            SqlDataReader drDB = cmd.ExecuteReader();
            while (drDB.Read())
            {
                cmDB.Items.Add(drDB["name"]);
            }
            con.Close();
        }

        private void cmDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmWave.Items.Clear();

            /*Выпадающий список Этапа*/
            con = new SqlConnection(_c);
            con.Open();
            cmd = new SqlCommand(@"use " + cmDB.Text.ToString() + " SELECT [WaveCode],[WaveName] FROM [dat_Waves] order by 1", con);

            SqlDataReader drWave = cmd.ExecuteReader();
            while (drWave.Read())
            {
                cmWave.Items.Add(drWave["WaveName"]);
            }
            con.Close();

            
        }

        private void cmWave_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmGov.Items.Clear();

            con = new SqlConnection(_c);
            con.Open();
            cmd2 = new SqlCommand(@"use " + cmDB.Text.ToString() + @" select
                                                                      g.GovernmentCode
                                                                      from
                                                                      rbd_Participants as p
                                                                      inner join rbd_Schools as s on s.SchoolID = p.SchoolRegistration
                                                                      inner join rbd_Governments as g on g.GovernmentID = s.GovernmentID
                                                                      inner join rbd_ParticipantsExams as pe on pe.ParticipantID = p.ParticipantID
                                                                      inner join dat_Exams as e on e.ExamGlobalID = pe.ExamGlobalID
                                                                      inner join dat_Waves as w on w.WaveCode = e.WaveCode
                                                                      where p.DeleteType = 0
                                                                      and s.DeleteType = 0
                                                                      and e.SubjectCode not in (20)
                                                                      and w.WaveName = '" + cmWave.Text.ToString() + "' " +
                                                                      "group by g.GovernmentCode " +
                                                                      "order by 1", con);
            SqlDataReader drGov = cmd2.ExecuteReader();
            while (drGov.Read())
            {
                cmGov.Items.Add(drGov["GovernmentCode"]);
            }
            con.Close();

            /*Список дат*/
            cmExamDate.Items.Clear();

            con = new SqlConnection(_c);
            con.Open();
            cmd3 = new SqlCommand(@"use " + cmDB.Text.ToString() + " SELECT distinct [ExamDate] " +
                                   "FROM [dat_Exams] as e " +
                                   "inner join dat_Waves as w on w.WaveCode = e.WaveCode " +
                                   "where TestTypeCode = 6 and w.WaveName = '" + cmWave.Text.ToString() + "' order by 1", con);
            SqlDataReader dr = cmd3.ExecuteReader();
            while (dr.Read())
            {
                cmExamDate.Items.Add(dr["ExamDate"]);
            }
            con.Close();

            /*Основной/Резервный день*/
            cmExamType.Items.Clear();

            con = new SqlConnection(_c);
            con.Open();
            cmd4 = new SqlCommand(@"use " + cmDB.Text.ToString() + " SELECT distinct [ExamType] " +
                                   "FROM [dat_Exams] as e " +
                                   "inner join dat_Waves as w on w.WaveCode = e.WaveCode " +
                                   "where TestTypeCode = 6 and w.WaveName = '" + cmWave.Text.ToString() + "' order by 1", con);
            SqlDataReader drET = cmd4.ExecuteReader();
            while (drET.Read())
            {
                cmExamType.Items.Add(drET["ExamType"]);
            }
            con.Close();
        }

        private void cmGov_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmSch.Items.Clear();

            con = new SqlConnection(_c);
            con.Open();
            cmd2 = new SqlCommand(@"use " + cmDB.Text.ToString() + @" select
                                                                      case when len(s.SchoolCode) = 5 then '0' + convert(varchar, s.SchoolCode) + ' - ' + s.ShortName else convert(varchar, s.SchoolCode) + ' - ' + s.ShortName end as [Schools]
                                                                      from
                                                                      rbd_Participants as p
                                                                      inner join rbd_Schools as s on s.SchoolID = p.SchoolRegistration
                                                                      inner join rbd_Governments as g on g.GovernmentID = s.GovernmentID
                                                                      inner join rbd_ParticipantsExams as pe on pe.ParticipantID = p.ParticipantID
                                                                      inner join dat_Exams as e on e.ExamGlobalID = pe.ExamGlobalID
                                                                      inner join dat_Waves as w on w.WaveCode = e.WaveCode
                                                                      where p.DeleteType = 0
                                                                      and s.DeleteType = 0
                                                                      and e.SubjectCode not in (20)
                                                                      and w.WaveName = '" + cmWave.Text.ToString() + "' " +
                                                                      " and g.GovernmentCode = " + cmGov.Text.ToString() + " " +
                                                                      "group by case when len(s.SchoolCode) = 5 then '0' + convert(varchar, s.SchoolCode) + ' - ' + s.ShortName else convert(varchar, s.SchoolCode) + ' - ' + s.ShortName end " +
                                                                      "order by 1", con);
            SqlDataReader drSch = cmd2.ExecuteReader();
            while (drSch.Read())
            {
                cmSch.Items.Add(drSch["Schools"]);
            }
            con.Close();
        }

        private void createDeclar_Click(object sender, EventArgs e)
        {
            
            if (cmDB.Text.ToString() != "" && cmWave.Text.ToString() != "") 
            {
                string gov = "";
                string sch = "";
                string exd = "";
                string ext = "";

                if (cmGov.Text.ToString() != "")
                {
                    gov += " and g.GovernmentCode = " + cmGov.Text.ToString();
                }

                if (cmSch.Text.ToString() != "")
                {
                    sch += " and s.SchoolCode = '" + cmSch.Text.ToString().Substring(0, 6) + "' ";
                }

                if (cmExamDate.Text.ToString() != "")
                {
                    exd += " and e.ExamDate = '" + cmExamDate.Text.ToString() + "' ";
                }

                if (cmExamType.Text.ToString() != "")
                {
                    ext += " and e.ExamType = " + cmExamType.Text.ToString() + " ";
                }

                con = new SqlConnection(_c);
                con.Open();
                cmd = new SqlCommand(@"use " + cmDB.Text.ToString() + @" select
                                p.ParticipantID
                                ,p.Surname
                                ,p.Name
                                ,p.SecondName
                                ,p.DocumentSeries
                                ,p.DocumentNumber
                                ,p.pClass
                                ,case when len(g.GovernmentCode) = 1 then '0' + convert(varchar, g.GovernmentCode) else convert(varchar, g.GovernmentCode) end as [GovernmentCode]
                                ,case when len(s.SchoolCode) = 5 then '0' + convert(varchar, s.SchoolCode) else convert(varchar, s.SchoolCode) end as [SchoolCode]
                                ,s.ShortName
                                ,p.ParticipantCode
                                from
                                rbd_Participants as p
                                inner join rbd_Schools as s on s.SchoolID = p.SchoolRegistration
                                inner join rbd_Governments as g on g.GovernmentID = s.GovernmentID
                                inner join rbd_ParticipantsExams as pe on pe.ParticipantID = p.ParticipantID
                                inner join dat_Exams as e on e.ExamGlobalID = pe.ExamGlobalID
                                inner join dat_Waves as w on w.WaveCode = e.WaveCode
                                where p.DeleteType = 0
                                and s.DeleteType = 0
                                and e.SubjectCode not in (20)
                                and w.WaveName = '" + cmWave.Text.ToString() + "' " +
                                gov +
                                sch +
                                exd + 
                                ext + 

                                " group by " +
                                "p.ParticipantID " +
                                ",p.Surname " +
                                ",p.Name " +
                                ",p.SecondName " +
                                ",p.DocumentSeries " +
                                ",p.DocumentNumber " +
                                ",p.pClass " +
                                ",g.GovernmentCode " +
                                ",s.SchoolCode " +
                                ",s.ShortName " +
                                ",p.ParticipantCode", con);
                cmd.ExecuteNonQuery();
                DataTable dtPart = new DataTable();
                SqlDataAdapter daPart = new SqlDataAdapter(cmd);
                daPart.Fill(dtPart);

                progressBar1.Visible = true;
                progressBar1.Minimum = 1;
                progressBar1.Maximum = dtPart.Rows.Count;
                progressBar1.Value = 1;
                progressBar1.Step = 1;

                for (int i = 0; i < dtPart.Rows.Count; i++)
                {
                    var pdfDoc = new Document(PageSize.A4, 35f, 35f, 40f, 35f);
                    //PdfWriter.GetInstance(pdfDoc, new FileStream(@"" + dtPart.Rows[i]["SchoolCode"].ToString() + "_" + dtPart.Rows[i]["Surname"].ToString() + "." + dtPart.Rows[i]["Name"].ToString().Substring(0, 1) + "." + dtPart.Rows[i]["SecondName"].ToString().Substring(0, 1) + ".pdf", FileMode.OpenOrCreate));
                    //var pdfFile = PdfWriter.GetInstance(pdfDoc, new FileStream(@"" + dtPart.Rows[i]["SchoolCode"].ToString() + "_" + dtPart.Rows[i]["Surname"].ToString() + "." + dtPart.Rows[i]["Name"].ToString().Substring(0, 1) + "." + dtPart.Rows[i]["SecondName"].ToString().Substring(0, 1) + ".pdf", FileMode.OpenOrCreate));
                    Directory.CreateDirectory(@"" + dtPart.Rows[i]["GovernmentCode"].ToString() + "\\" + dtPart.Rows[i]["SchoolCode"].ToString());
                    //PdfWriter.GetInstance(pdfDoc, new FileStream(@"" + dtPart.Rows[i]["GovernmentCode"].ToString() + "\\" + dtPart.Rows[i]["SchoolCode"].ToString() + "\\" + dtPart.Rows[i]["SchoolCode"].ToString() + "_" + dtPart.Rows[i]["Surname"].ToString() + "." + dtPart.Rows[i]["Name"].ToString().Substring(0, 1) + "." + dtPart.Rows[i]["SecondName"].ToString().Substring(0, 1) + ".pdf", FileMode.OpenOrCreate));
                    PdfWriter.GetInstance(pdfDoc, new FileStream(@"" + dtPart.Rows[i]["GovernmentCode"].ToString() + "\\" + dtPart.Rows[i]["SchoolCode"].ToString() + "\\" + dtPart.Rows[i]["SchoolCode"].ToString() + "_" + dtPart.Rows[i]["Surname"].ToString() + "." + dtPart.Rows[i]["Name"].ToString().Substring(0, 1) + "_" + i.ToString() + ".pdf", FileMode.OpenOrCreate));
                    pdfDoc.Open();

                    #region Отступы
                    var spacer30 = new Paragraph("")
                    {
                        SpacingAfter = 30f,
                    };
                    var spacer10 = new Paragraph("")
                    {
                        SpacingAfter = 10f,
                    };
                    var spacer5 = new Paragraph("")
                    {
                        SpacingAfter = 5f,
                    };
                    #endregion

                    Paragraph headOrder = new Paragraph();
                    BaseFont bf = BaseFont.CreateFont("c:/Windows/Fonts/times.ttf", BaseFont.IDENTITY_H, false);
                    iTextSharp.text.Font fntHeadOrder = new iTextSharp.text.Font(bf, 18, 1, iTextSharp.text.BaseColor.BLACK); // Жирный текст
                    iTextSharp.text.Font fntFootNote = new iTextSharp.text.Font(bf, 12, 0, iTextSharp.text.BaseColor.BLACK);
                    headOrder.Alignment = Element.ALIGN_CENTER;
                    headOrder.Add(new Chunk("УВЕДОМЛЕНИЕ", fntHeadOrder));
                    pdfDoc.Add(headOrder);

                    pdfDoc.Add(spacer5);

                    PdfPTable tabInfom = new PdfPTable(4); //Кол-во столбцов
                    BaseFont bfTableHeader = BaseFont.CreateFont("c:/Windows/Fonts/times.ttf", BaseFont.IDENTITY_H, false);
                    iTextSharp.text.Font fntTableHeader = new iTextSharp.text.Font(bfTableHeader, 14, 1, iTextSharp.text.BaseColor.BLACK); // Жирный текст
                    iTextSharp.text.Font fntTableContent = new iTextSharp.text.Font(bfTableHeader, 14, 0, iTextSharp.text.BaseColor.BLACK); // Простой текст
                    tabInfom.WidthPercentage = 100;
                    tabInfom.DefaultCell.HorizontalAlignment = 1;
                    tabInfom.DefaultCell.VerticalAlignment = 1;
                    /*Шапка*/
                    PdfPCell cell = new PdfPCell(new Phrase(@"Информация участника ГИА-9 о регистрации на экзамены (уведомление необходимо оставить в месте для хранения личных вещей участников ГИА-9 или отдать сопровождающему *)", fntTableHeader)) { PaddingBottom = 7 };
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);

                    /*Участник*/
                    cell = new PdfPCell(new Phrase(@"Информация об участнике ГИА-9", fntTableHeader)) { PaddingBottom = 7 };
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    /*Фамилия*/
                    cell = new PdfPCell(new Phrase("Фамилия", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["Surname"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    tabInfom.AddCell(cell);
                    /*Документ заголовок*/
                    cell = new PdfPCell(new Phrase(@"Документ", fntTableHeader)) { PaddingBottom = 7 };
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);

                    /*Имя*/
                    cell = new PdfPCell(new Phrase("Имя", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["Name"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    tabInfom.AddCell(cell);
                    /*Серия*/
                    cell = new PdfPCell(new Phrase("Серия", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["DocumentSeries"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    tabInfom.AddCell(cell);

                    /*Отчество*/
                    cell = new PdfPCell(new Phrase("Отчество", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["SecondName"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    tabInfom.AddCell(cell);
                    /*Номер*/
                    cell = new PdfPCell(new Phrase("Номер", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["DocumentNumber"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    tabInfom.AddCell(cell);

                    /*Код ОО */
                    cell = new PdfPCell(new Phrase("Код ОО", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["SchoolCode"].ToString() + " - " + dtPart.Rows[i]["ShortName"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    cell.Colspan = 3;
                    tabInfom.AddCell(cell);



                    /*Код регистрации и класс*/
                    cell = new PdfPCell(new Phrase("Код регистрации", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["ParticipantCode"].ToString().Substring(0, 4) + "-" + dtPart.Rows[i]["ParticipantCode"].ToString().Substring(4, 4) + "-" + dtPart.Rows[i]["ParticipantCode"].ToString().Substring(8, 4), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    tabInfom.AddCell(cell);

                    cell = new PdfPCell(new Phrase("Класс", fntTableHeader)) { PaddingBottom = 7 };
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabInfom.AddCell(cell);
                    cell = new PdfPCell(new Phrase(dtPart.Rows[i]["pClass"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                    tabInfom.AddCell(cell);

                    pdfDoc.Add(tabInfom);

                    pdfDoc.Add(spacer30);

                    con = new SqlConnection(_c);
                    con.Open();
                    cmd = new SqlCommand(@"use " + cmDB.Text.ToString() + @" select
                                    p.ParticipantID
                                    ,case when len(ss.StationCode) = 3 then '0' + convert(varchar, ss.StationCode) else convert(varchar, ss.StationCode) end as [StationCode]
                                    ,ss.StationAddress
                                    ,ss.StationName
                                    ,e.ExamDate
                                    ,case when len(e.SubjectCode) = 1 then '0' + convert(varchar, e.SubjectCode) else convert(varchar, e.SubjectCode) end as [SubjectCode]
                                    ,sub.SubjectName
                                    from
                                    rbd_Participants as p
                                    inner join rbd_Schools as s on s.SchoolID = p.SchoolRegistration
                                    inner join rbd_Governments as g on g.GovernmentID = s.GovernmentID
                                    inner join rbd_ParticipantsExams as pe on pe.ParticipantID = p.ParticipantID
                                    inner join dat_Exams as e on e.ExamGlobalID = pe.ExamGlobalID
                                    inner join dat_Waves as w on w.WaveCode = e.WaveCode
                                    inner join dat_Subjects as sub on sub.SubjectCode = e.SubjectCode
                                    inner join rbd_ParticipantsExamsOnStation as peos on peos.ParticipantsExamsID = pe.ParticipantsExamsID
                                    inner join rbd_StationsExams as se on se.StationsExamsID = peos.StationsExamsID
                                    inner join rbd_Stations as ss on ss.StationID = se.StationID
                                    where p.DeleteType = 0
                                    and e.SubjectCode not in (20)
                                    --and e.SubjectCode in (1,2,51,52)
                                    and w.WaveName = '" + cmWave.Text.ToString() + "' " +
                                    gov +
                                    sch +
                                    exd +
                                    ext +
                                    "and p.ParticipantID = '" + dtPart.Rows[i]["ParticipantID"].ToString() + "'", con);
                    cmd.ExecuteNonQuery();
                    DataTable dtPartExam = new DataTable();
                    SqlDataAdapter daPartExam = new SqlDataAdapter(cmd);
                    daPartExam.Fill(dtPartExam);

                    for (int j = 0; j < dtPartExam.Rows.Count; j++)
                    {
                        /*Экзамены*/
                        Paragraph exam = new Paragraph();
                        exam.Add(new Chunk("     "));
                        pdfDoc.Add(exam);

                        PdfPTable tabExam = new PdfPTable(4); //Кол-во столбцов
                        tabExam.WidthPercentage = 100;
                        tabExam.DefaultCell.HorizontalAlignment = 1;
                        tabExam.DefaultCell.VerticalAlignment = 1;

                        cell = new PdfPCell(new Phrase("Экзамен (ГИА-9)", fntTableHeader)) { PaddingBottom = 7 };
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase(dtPartExam.Rows[j]["SubjectName"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Код предмета", fntTableHeader)) { PaddingBottom = 7 };
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase(dtPartExam.Rows[j]["SubjectCode"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        tabExam.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Дата проведения", fntTableHeader)) { PaddingBottom = 7 };
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase(dtPartExam.Rows[j]["ExamDate"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                        cell.Colspan = 3;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        tabExam.AddCell(cell);

                        cell = new PdfPCell(new Phrase(@"Пункт проведения экзамена (ППЭ)", fntTableHeader)) { PaddingBottom = 7 };
                        cell.Colspan = 2;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase("Код ППЭ", fntTableHeader)) { PaddingBottom = 7 };
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase(dtPartExam.Rows[j]["StationCode"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        tabExam.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Адрес", fntTableHeader)) { PaddingBottom = 7 };
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase(dtPartExam.Rows[j]["StationAddress"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                        cell.Colspan = 3;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        tabExam.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Наименование", fntTableHeader)) { PaddingBottom = 7 };
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabExam.AddCell(cell);
                        cell = new PdfPCell(new Phrase(dtPartExam.Rows[j]["StationName"].ToString(), fntTableContent)) { PaddingBottom = 7, PaddingLeft = 5 };
                        cell.Colspan = 3;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        tabExam.AddCell(cell);

                        pdfDoc.Add(tabExam);
                    }

                    Paragraph footnote = new Paragraph();
                    footnote.Add(new Chunk("* С момента входа в ППЭ и до окончания экзамена участникам ГИА-9 запрещено иметь при себе уведомление", fntFootNote));
                    pdfDoc.Add(footnote);

                    pdfDoc.Close();

                    progressBar1.PerformStep();
                }

                MessageBox.Show("Заявление сформировано!");
                //MessageBox.Show("Выбрано поле БД и Этап");
            }
            else
            {
                MessageBox.Show("Не выбрано поле БД и Этап");
            }
                
            
            

            
        }

        
    }
}

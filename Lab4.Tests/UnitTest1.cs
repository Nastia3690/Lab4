namespace Lab4.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Notebook notebook = new Notebook();
            List<Contact> FoundRecords = new List<Contact>();
            Predicate<Contact> predicate;
            Assert.True(notebook.Add("Алексей", "Васильев", "+79219451233", "alexvas@mail.ru"));
            Assert.True(notebook.Add("Александр", "Муравьев", "+79637492654", "amuravei@mail.ru"));
            Assert.True(notebook.Add("Артем", "Петров", "+79214425635", "artemp@yandex.ru"));
            Assert.True(notebook.Add("Артур", "Иванов", "+79377742653", "arturivanov@mail.ru"));
            Assert.True(notebook.Add("Борис", "Артемьев", "+79374766301", "borisart@mail.ru"));
            Assert.True(notebook.Add("Василий", "Серов", "+79619463593", "vvserov@yandex.ru"));
            Assert.True(notebook.Add("Сергей", "Быков", "+79617443583", "sergbykov@mail.ru"));
            Assert.True(notebook.Add("Геннадий", "Букин", "+79627253254", "genbukin@mail.ru"));
            Assert.True(notebook.Add("Сергей", "Даров", "+79987742600", "sdarov@yandex.ru"));
            Assert.True(notebook.Add("Евгений", "Пашков", "+79607545463", "evgpashkov@mail.ru"));
            Assert.True(notebook.Add("Иван", "Сергеев", "+79677222333", "isergeev@yandex.ru"));
            FoundRecords = notebook.AllRecords;
            Assert.Equal(11, FoundRecords.Count);

            predicate = contact => contact.Name.Contains("Арт");
            FoundRecords = notebook.Search(predicate);
            Assert.Equal(2, FoundRecords.Count);
            Assert.Equal("Петров", FoundRecords[0].Surname);
            Assert.Equal("Иванов", FoundRecords[1].Surname);

            predicate = contact => contact.Surname.Contains("Сер");
            FoundRecords = notebook.Search(predicate);
            Assert.Equal(2, FoundRecords.Count);
            Assert.Equal("Серов", FoundRecords[0].Surname);
            Assert.Equal("Сергеев", FoundRecords[1].Surname);

            predicate = contact => contact.Name.Contains("Арт") || contact.Surname.Contains("Арт");
            FoundRecords = notebook.Search(predicate);
            Assert.Equal(3, FoundRecords.Count);
            Assert.Equal("Петров", FoundRecords[0].Surname);
            Assert.Equal("Иванов", FoundRecords[1].Surname);
            Assert.Equal("Артемьев", FoundRecords[2].Surname);

            predicate = contact => contact.Phone.Contains("77");
            FoundRecords = notebook.Search(predicate);
            Assert.Equal(3, FoundRecords.Count);
            Assert.Equal("Иванов", FoundRecords[0].Surname);
            Assert.Equal("Даров", FoundRecords[1].Surname);
            Assert.Equal("Сергеев", FoundRecords[2].Surname);

            predicate = contact => contact.Email.Contains("yandex.ru");
            FoundRecords = notebook.Search(predicate);
            Assert.Equal(4, FoundRecords.Count);
            Assert.Equal("Петров", FoundRecords[0].Surname);
            Assert.Equal("Серов", FoundRecords[1].Surname);
            Assert.Equal("Даров", FoundRecords[2].Surname);
            Assert.Equal("Сергеев", FoundRecords[3].Surname);

            //SAVE
            Assert.True(notebook.SaveToXML());
            Assert.True(notebook.SaveToJSON());
            Assert.True(notebook.SaveToSQLite());
            //XML
            notebook.Clear();
            Assert.Equal(0, notebook.Count());
            Assert.True(notebook.LoadFromXML());
            Assert.Equal(11, notebook.Count());
            //JSON
            notebook.Clear();
            Assert.True(notebook.LoadFromJSON());
            Assert.Equal(11, notebook.Count());
            //SQLite
            notebook.Clear();
            Assert.True(notebook.LoadFromSQLite());
            Assert.Equal(11, notebook.Count());
        }
    }
}
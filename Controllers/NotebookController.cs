using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Lab4.Controllers
{
    public class NotebookController : ApiController
    {

        // GET api/values
        // возвратить список всех контактов или найденных, если был поиск
        public IEnumerable<Contact> Get()
        {
            if (WebApiApplication.AllRecords)
            {
                return WebApiApplication.notebook.AllRecords.AsEnumerable();
            }
            else
            {
                return WebApiApplication.FoundRecords.AsEnumerable();
            }
        }
        // GET api/values
        // возвратить контакт по индексу index
        public string Get(int index)
        {
            if (WebApiApplication.AllRecords)
            {
                if (index < WebApiApplication.notebook.Count())
                {
                    return WebApiApplication.notebook.AllRecords[index].ToString();
                }
            }
            else
            {
                if (index < WebApiApplication.FoundRecords.Count())
                {
                    return WebApiApplication.FoundRecords[index].ToString();
                }
            }
            return "";
        }
        // POST api/values
        // Список параметров value, первый параметр команда
        public IHttpActionResult Post([FromBody] List<String> value)
        {
            Predicate<Contact> predicate;
            if (value.Count == 0)
            {
                return this.StatusCode(HttpStatusCode.NotAcceptable);
            }
            else
            if (value[0] == "allcontacts" && value.Count == 1)//показать все контакты
            {
                WebApiApplication.AllRecords = true;
            }
            else
            if (value[0] == "newcontact" && value.Count == 5)//Добавить контакт
            {
                if (WebApiApplication.notebook.Add(value[1], value[2], value[3], value[4]))
                {
                    WebApiApplication.AllRecords = true;
                }
            }
            else
            if (value[0] == "byname" && value.Count == 2)//поиск по имени
            {
                WebApiApplication.AllRecords = false;
                predicate = contact => contact.Name.Contains(value[1]);
                WebApiApplication.FoundRecords = WebApiApplication.notebook.Search(predicate);
            }
            else
            if (value[0] == "bysurname" && value.Count == 2)//поиск по фамилии
            {
                WebApiApplication.AllRecords = false;
                predicate = contact => contact.Surname.Contains(value[1]);
                WebApiApplication.FoundRecords = WebApiApplication.notebook.Search(predicate);
            }
            else
            if (value[0] == "byfullname" && value.Count == 2)//поиск по имени и фамилии
            {
                WebApiApplication.AllRecords = false;
                predicate = contact => contact.Name.Contains(value[1]) || contact.Surname.Contains(value[1]);
                WebApiApplication.FoundRecords = WebApiApplication.notebook.Search(predicate);
            }
            else
            if (value[0] == "byphone" && value.Count == 2)//поиск по автору
            {
                WebApiApplication.AllRecords = false;
                predicate = contact => contact.Phone.Contains(value[1]);
                WebApiApplication.FoundRecords = WebApiApplication.notebook.Search(predicate);
            }
            else
            if (value[0] == "byemail" && value.Count == 2)//поиск по автору
            {
                WebApiApplication.AllRecords = false;
                predicate = contact => contact.Email.Contains(value[1]);
                WebApiApplication.FoundRecords = WebApiApplication.notebook.Search(predicate);
            }
            else
            if (value[0] == "savetoxml" && value.Count == 1)
            {
                WebApiApplication.notebook.SaveToXML();
            }
            else
            if (value[0] == "loadfromxml" && value.Count == 1)
            {
                WebApiApplication.AllRecords = WebApiApplication.notebook.LoadFromXML() || WebApiApplication.AllRecords;
            }
            else
            if (value[0] == "savetojson" && value.Count == 1)
            {
                WebApiApplication.notebook.SaveToJSON();
            }
            else
            if (value[0] == "loadfromjson" && value.Count == 1)
            {
                WebApiApplication.AllRecords = WebApiApplication.notebook.LoadFromJSON() || WebApiApplication.AllRecords;
            }
            else
            if (value[0] == "savetosqlite" && value.Count == 1)
            {
                WebApiApplication.notebook.SaveToSQLite();
            }
            else
            if (value[0] == "loadfromsqlite" && value.Count == 1)
            {
                WebApiApplication.AllRecords = WebApiApplication.notebook.LoadFromSQLite() || WebApiApplication.AllRecords;
            }
            else
            {
                return this.StatusCode(HttpStatusCode.NotAcceptable);
            }
            return this.StatusCode(HttpStatusCode.OK);
        }

        // PUT api/values
        // изменить контакт с индексом index, строка с полями value
        public IHttpActionResult Put(int index, [FromBody] string value)
        {
            if (WebApiApplication.notebook.UpdateContact(index, value))
            {
                return this.StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return this.StatusCode(HttpStatusCode.NotAcceptable);
            }
        }

        // DELETE api/values
        // удалить контакт с индексом index
        public IHttpActionResult Delete(int index)
        {
            if (WebApiApplication.notebook.RemoveContact(index))
            {
                return this.StatusCode(HttpStatusCode.OK);
            }
            else
            {
                return this.StatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}

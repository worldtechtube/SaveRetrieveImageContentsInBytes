using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi1.Models;

namespace WebApi1.Controllers
{
    public class FilesController : ApiController
    {
        SqlCommand cmd;
        SqlConnection con;
        FilesController()
        {
            cmd = new SqlCommand();
            con = new SqlConnection(@"Data Source=DESKTOP-M3EM8R3\MSSQLSERVER01;Initial Catalog=StudentDB;Integrated Security=True");
        }
        
        // GET api/values
        public IEnumerable<Wallpaper> Get()
        {
            var wallpapers = new List<Wallpaper>();
            cmd.Connection = con;
            cmd.Connection.Open();
            cmd.CommandText = "select id, wallpaperContent from wallpaper where id = 2";
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                byte[] imgBytes = (byte[])dr["wallpaperContent"];
                var wallpaper = new Wallpaper
                {
                    id = Convert.ToInt32(dr.GetValue(0)),
                    wallpaperContent = imgBytes
                };
                wallpapers.Add(wallpaper);
            }

            return wallpapers;
        }

        // GET api/values/2
        public HttpResponseMessage Get(int id)
        {            
            cmd.Connection = con;
            cmd.Connection.Open();
            cmd.CommandText = "select id, wallpaperContent from wallpaper where id = "+id;
            SqlDataReader dr = cmd.ExecuteReader();
            HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.OK);
            if (dr.HasRows)
            {
                dr.Read();
                byte[] imgBytes = (byte[])dr["wallpaperContent"];
                if (imgBytes == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }               
                Response.Content =  new StreamContent(new MemoryStream(imgBytes));
                Response.Content.Headers.ContentType = 
                        new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpg");
                return Response;
            }

            throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        // POST api/values
        public void Post()
        {
            // Check if the request contains multipart/form-data.  
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var file = HttpContext.Current.Request.Files[0].InputStream;
            byte[] wallpaperImage;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                wallpaperImage = memoryStream.ToArray();
                using (cmd = new SqlCommand("Wallpaper_insert", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", 1);
                    cmd.Parameters.AddWithValue("@wallpaperContent", wallpaperImage);
                }
                con.Open();
                var newId = cmd.ExecuteNonQuery();
                con.Close();                
            }
        }
    }
}

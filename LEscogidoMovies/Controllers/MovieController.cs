using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LEscogidoMovies.Controllers
{
    public class MovieController : Controller
    {
        public ActionResult Index()
        {
            ML.Movie movie = new ML.Movie();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.themoviedb.org/3/");
                var responseTask = client.GetAsync("account/16815182/favorite/movies?api_key=4a68b38ec74d1ca2925f2665e1fafdb4&session_id=b5eaf43b500fbe601cc10ad4e34af74538f78fd7&language=es-ES&sort_by=created_at.asc&page=1");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    dynamic resultJSON = JObject.Parse(readTask.Result.ToString());
                    readTask.Wait();
                    movie.Movies = new List<object>();
                    foreach (var resultItem in resultJSON.results)
                    {
                        ML.Movie movieItem = new ML.Movie();
                        movieItem.Descripcion = resultItem.overview;
                        movieItem.Nombre = resultItem.title;
                        movieItem.Imagen = "https://www.themoviedb.org/t/p/w600_and_h900_bestv2" + resultItem.poster_path;
                        movie.Movies.Add(movieItem);
                    }
                }
            }
            return View(movie);
        }
    }
}

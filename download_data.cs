using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace omdb_to_list
{

    public class GetMovieList : EventArgs
    {
        public String data{ get; set; }
    }

    class download_data
    {

        public event EventHandler<GetMovieList> getMovies;

        public void get_data(String url)
        {
            MovieDataAsync(url);
        }


        async void MovieDataAsync(String url)
        {
            HttpClient client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync(url);
            string urlContents = await getStringTask;

            GetMovieList data_to_send = new GetMovieList();
            data_to_send.data = urlContents;

            GetMovieListEvent(data_to_send);

        }

        protected virtual void GetMovieListEvent(GetMovieList e)
        {
            EventHandler<GetMovieList> handler = getMovies;
            if (handler != null)
            {
                handler(this, e);
            }
        }


    }
}

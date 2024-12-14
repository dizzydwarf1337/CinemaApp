import { RouteObject, createBrowserRouter } from "react-router-dom";
import App  from "../layout/App";
import CinemaDashboard from "../../features/cinema/cinemaDashboard";
import SessionsDashboard from "../../features/sessions/sessionsDashboard";
import MovieDashboard from "../../features/movies/movieDashboard";
import CinemaDetails from "../../features/cinema/cinemaDetails/cinemaDetails";
import MovieDetails from "../../features/movies/movieDetails/movieDetails";
import CinemaForm from "../../features/cinema/Form/cinemaForm";
import MovieForm from "../../features/movies/form/movieForm";
import SessionForm from "../../features/sessions/form/sessionForm";
import HomePage from "../../features/home/homePage";
import TicketsDashboard from "../../features/tickets/ticketsDashboard";


 export const routes: RouteObject[] = [
    {
        path: "/",
         element: <App />,
         children: [
             {
                 path: "/cinema",
                 element: <CinemaDashboard/>,
             },
             {
                 path: "/",
                 element: <HomePage/>
             },
             {
                 path: "/session",
                 element: <SessionsDashboard/>
             },
             {
                 path: "/movie",
                 element: <MovieDashboard/>
             },
             {
                 path: "/cinema/:id",
                 element: <CinemaDetails/>
             },
             {
                 path: "/movie/:id",
                 element: <MovieDetails/>
             },
             {
                 path: "/cinema/manage/:id",
                 element: <CinemaForm/>
             },
             {
                 path: "/cinema/manage/",
                 element: <CinemaForm />
             },
             {
                 path: "/movie/manage/:id",
                 element:<MovieForm/>
             },
             {
                 path: "/movie/manage/",
                 element: <MovieForm />
             },
             {
                 path: "/session/manage/",
                 element:<SessionForm/>
             },
             {
                 path: "/session/manage/:id",
                 element: <SessionForm />
             },
             {
                 path: "/myTickets",
                 element: <TicketsDashboard/>
             },
         ]
    },
]

export const router = createBrowserRouter(routes);
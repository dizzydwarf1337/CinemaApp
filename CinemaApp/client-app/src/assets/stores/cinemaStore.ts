import { makeAutoObservable } from "mobx";
import cinema from "../models/cinema";
import hall from "../models/hall";
import session from "../models/session";
import movie from "../models/movie";
import agent from "../API/agent";

export default class CinemaStore  {
    constructor() {
        makeAutoObservable(this);
        this.loadAllData();
    }
    cinemas: cinema[] = [];
    cinema: cinema | null = null;
    halls: hall[] = [];
    hall: hall | null = null;
    sessions: session[] = [];
    session: session | null = null;
    movies: movie[] = [];
    movie: movie | null = null;
    loading = false;
    setLoading(value: boolean) {
        this.loading = value;
    }
    getLoading = () => this.loading;
    getCinemas = () => this.cinemas;
    setCinemas = (cinemas: cinema[]) => {
        this.cinemas = cinemas;
    }
    loadCinemas = async () => {
        this.setLoading(true);
        agent.Cinemas.getCinemas().then((cinemas) => { this.setCinemas(cinemas) }).
            catch((error) => { console.log("error while loadingCinemas ", error) })
            .finally(() =>this.setLoading(false));
    }
    createCinema = async (cinema: cinema) => {
        this.setLoading(true);
        agent.Cinemas.createCinema(cinema).then(() => { this.setCinemas([...this.cinemas, cinema]) })
            .catch((error) => { console.log("error while creating cinema: ", error) })
            .finally(()=>this.setLoading(false));
    }
    deleteCinema = async (id: string) => {
        this.setLoading(true);
        agent.Cinemas.deleteCinema(id).then(() => { this.setCinemas(this.cinemas.filter(x => x.id !== id)) })
            .catch((error) => { console.log("error while deleting cinema: ", error) })
            .finally(() => this.setLoading(false));
    }
    updateCinema = async (cinema: cinema) => {
        this.setLoading(true);
        agent.Cinemas.updateCinema(cinema).then(() => { this.setCinemas([...this.cinemas.filter(x => x.id !== cinema.id), cinema]) })
            .catch((error) => { console.log("error while updating cinema: ", error) })
            .finally(() => this.setLoading(false));
    }

    getCinema = () => this.cinema;
    setCinema = (cinema: cinema) => {
        this.cinema = cinema;
    }


    getHalls = () => this.halls;
    setHalls = (halls: hall[]) => {
        this.halls = halls;
    }
    loadHalls = async () => {
        this.setLoading(true);
        agent.Halls.getHalls().then((halls) => { this.setHalls(halls) })
            .catch((error) => { console.log("error while loadingHalls ", error) })
            .finally(() => { this.setLoading(false) });
    }
    createHall = async (hall: hall) => {
        this.setLoading(true);
        agent.Halls.createHall(hall).then(() => { this.setHalls([...this.halls, hall]) })
            .catch((error) => { console.log("error while creating hall: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    deleteHall = async (id: string) => {
        this.setLoading(true);
        agent.Halls.deleteHall(id).then(() => { this.setHalls(this.halls.filter(x => x.id !== id)) })
            .catch((error) => { console.log("error while deleting hall: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    updateHall = async (hall: hall) => {
        this.setLoading(true);
        agent.Halls.updateHall(hall).then(() => { this.setHalls([...this.halls.filter(x => x.id !== hall.id), hall]) })
            .catch((error) => { console.log("error while updating hall: ", error) })
            .finally(() => { this.setLoading(false) });
    }

    getHall = () => {
        return this.hall;
    }
    setHall = (hall: hall) => {
        this.hall = hall;
    }


    getMovies = () => this.movies;
    setMovies = (movies: movie[]) => {
        this.movies = movies;
    }
    loadMovies = async () => {
        this.setLoading(true);
        agent.Movies.getMovies().then((movies) => { this.setMovies(movies); })
            .catch((error) => { console.log("error while loading movies: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    createMovie = async (movie: movie) => {
        this.setLoading(true);
        agent.Movies.createMovie(movie).then(() => { this.setMovies([...this.movies, movie]) })
            .catch((error) => { console.log("error while creating movie: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    deleteMovie = async (id: string) => {
        this.setLoading(true);
        agent.Movies.deleteMovie(id).then(() => { this.setMovies(this.movies.filter(x => x.id !== id)) })
            .catch((error) => { console.log("error while deleting movie: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    updateMovie = async (movie: movie) => {
        this.setLoading(true);
        agent.Movies.updateMovie(movie).then(() => { this.setMovies([...this.movies.filter(x => x.id !== movie.id), movie]) })
            .catch((error) => { console.log("error while updating movie: ", error) })
            .finally(() => { this.setLoading(false) });
    }

    setMovie = (movie: movie) => {
        this.movie = movie;
    }
    getMovie = () => this.movie;


    getSessions = () => this.sessions;
    setSessions = (sessions: session[]) => {
        this.sessions = sessions;
    }
    loadSessions = async () => {
        this.setLoading(true);
        agent.Sessions.getSessions().then((sessions) => { this.setSessions(sessions); })
            .catch((error) => { console.log("error while loading sessions: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    createSession = async (session: session) => {
        this.setLoading(true);
        agent.Sessions.createSession(session).then(() => { this.setSessions([...this.sessions, session]) })
            .catch((error) => { console.log("error while creating session: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    deleteSession = async (id: string) => {
        this.setLoading(true);
        agent.Sessions.deleteSession(id).then(() => { this.setSessions(this.sessions.filter(x => x.id !== id)) })
            .catch((error) => { console.log("error while deleting session: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    updateSession = async (session: session) => {
        this.setLoading(true);
        agent.Sessions.updateSession(session).then(() => { this.setSessions([...this.sessions.filter(x => x.id !== session.id), session]) })
            .catch((error) => { console.log("error while updating session: ", error) })
            .finally(() => { this.setLoading(false) });
    }

    getSession = () => this.session;
    setSession = (session: session) => {
        this.session = session;
    }

    loadAllData = async () => {
        await Promise.all([
            this.loadCinemas(),
            this.loadHalls(),
            this.loadMovies(),
            this.loadSessions(),]);
    }
}
import { makeAutoObservable} from "mobx";
import cinema from "../models/cinema";
import hall from "../models/hall";
import session from "../models/session";
import movie from "../models/movie";
import agent from "../API/agent";

export default class CinemaStore  {
    constructor() {
        makeAutoObservable(this);
        this.setLoading(true);
        this.initialize();
        this.setLoading(false);
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
    getCinemaApi = async (id: string) => {
        this.setLoading(true);
        agent.Cinemas.getCinemaById(id).then((cinema) => { return cinema }).catch((error) => { console.log("error while getting cinema: ", error); return null; }).finally(() => this.setLoading(false));
    }
    loadCinemas = async () => {
        agent.Cinemas.getCinemas().then((cinemas) => { this.setCinemas(cinemas) }).
            catch((error) => { console.log("error while loadingCinemas ", error) });
    }
    createCinema = async (cinemaDto: cinema,numOfHalls:Number,file?:File) => {
        this.setLoading(true);
        console.log(cinemaDto, numOfHalls);
        agent.Cinemas.createCinema(cinemaDto, numOfHalls).then(async (cinema) => { if (file) { const formData = new FormData(); formData.append('formFile', file); await agent.Cinemas.uploadImage(cinema.id, formData); } this.setCinemas([...this.cinemas, cinema]) })
            .catch((error) => { console.log("error while creating cinema: ", error) })
            .finally(()=>this.setLoading(false));
    }
    deleteCinema = async (id: string) => {
        this.setLoading(true);
        agent.Cinemas.deleteCinema(id).then(() => { this.setCinemas(this.cinemas.filter(x => x.id !== id)) })
            .catch((error) => { console.log("error while deleting cinema: ", error) })
            .finally(() => this.setLoading(false));
    }
    updateCinema = async (cinema: cinema, file?:File) => {
        this.setLoading(true);
        cinema.imagePath = "";
        agent.Cinemas.updateCinema(cinema).then(async () => { if (file) { const formData = new FormData(); formData.append('formFile', file);  await agent.Cinemas.uploadImage(cinema.id, formData) }; this.setCinemas([...this.cinemas.filter(x => x.id !== cinema.id), cinema]) })
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
    getHallApi = async (id: string) => {
        this.setLoading(true);
        agent.Halls.getHallById(id).then((hall) => { return hall }).catch((error) => { console.log("error while getting hall: ", error) }).finally(() => this.setLoading(false));
    }
    loadHalls = async () => {
        agent.Halls.getHalls().then((halls) => { this.setHalls(halls) })
            .catch((error) => { console.log("error while loadingHalls ", error) });
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
    getMovieApi = async (id: string) => {
        this.setLoading(true);
        agent.Movies.getMovieById(id).then((movie) => { return movie }).catch((error) => { console.log("error while getting movie: ", error) }).finally(() => this.setLoading(false));
    }
    loadMovies = async () => {
        agent.Movies.getMovies().then((movies) => { this.setMovies(movies);  })
            .catch((error) => { console.log("error while loading movies: ", error) });
    }
    createMovie = async (movie: movie,file?:File) => {
        this.setLoading(true);
        
        agent.Movies.createMovie(movie).then(async () => { if (file) { const formData = new FormData(); formData.append('formFile', file); await agent.Movies.uploadImage(movie.id,formData) }; this.setMovies([...this.movies, movie]) })
            .catch((error) => { console.log("error while creating movie: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    deleteMovie = async (id: string) => {
        this.setLoading(true);
        agent.Movies.deleteMovie(id).then(() => { this.setMovies(this.movies.filter(x => x.id !== id)) })
            .catch((error) => { console.log("error while deleting movie: ", error) })
            .finally(() => { this.setLoading(false) });
    }
    updateMovie = async (movie: movie, file?:File) => {
        this.setLoading(true);
        agent.Movies.updateMovie(movie).then(async () => { if (file) { const formData = new FormData(); formData.append('formFile', file); await agent.Movies.uploadImage(movie.id, formData) }; this.setMovies([...this.movies.filter(x => x.id !== movie.id), movie]) })
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
    getSessionApi = async (id: string) => {
        this.setLoading(true);
        agent.Sessions.getSessionById(id).then((session) => { return session }).catch((error) => { console.log("error while getting session: ", error) }).finally(() => this.setLoading(false));
    }
    loadSessions = async () => {
        agent.Sessions.getSessions().then((sessions) => { this.setSessions(sessions); })
            .catch((error) => { console.log("error while loading sessions: ", error) });
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

    initialize = async () => {
        await this.loadCinemas();
        await this.loadHalls();
        await this.loadMovies();
        await this.loadSessions();
    }
}
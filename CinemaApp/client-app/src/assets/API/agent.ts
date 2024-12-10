import axios, { AxiosResponse } from 'axios';
import user from '../models/user';
import LoginModel from '../models/loginModel';
import cinema from '../models/cinema';
import hall from '../models/hall';
import movie from '../models/movie';
import session from '../models/session';
import ticket from '../models/ticket';

axios.defaults.baseURL = 'http://localhost:5000/api';

const responseBody = (response: AxiosResponse) => response.data;

const requests = {
    get: <T>(url: string) =>
        axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) =>
        axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) =>
        axios.put<T>(url, body,).then(responseBody),
    delete: <T>(url: string) =>
        axios.delete<T>(url).then(responseBody),
};


const Auth = {
    login: (loginModel: LoginModel) => requests.post<Response>('/login', loginModel),
};
const Users = {
    getUserById: (id: string) => requests.get<user>(`/user/${id}`),
    getUserByUserName: (userName: Object) => requests.post<user>('/user/userName/', { userName }),
    createUser: (user: user) => requests.post<void>('/user/createUser', user),
    deleteUser: (id: string) => requests.delete<void>(`/user/${id}`),
    updateUser: (user: user) => requests.put<void>(`/user/${user.id}`, user),
};
const Cinemas = {
    getCinemas: () => requests.get<cinema[]>('/cinema'),
    getCinemaById: (id: string) => requests.get<cinema>(`/cinema/${id}`),
    createCinema: (cinemaDto: cinema, numOfHalls: Number) => requests.post<cinema>(`/cinema`, { cinemaDto: cinemaDto, numOfHalls: numOfHalls }),
    updateCinema: (cinema: cinema) => requests.put<void>(`/cinema`, cinema),
    deleteCinema: (id: string) => requests.delete<void>(`/cinema/${id}`),
    uploadImage: (id: string, file: FormData) => requests.put<void>(`/cinema/${id}`, file)
};

const Halls = {
    getHalls: () => requests.get<hall[]>('/hall'),
    getHallById: (id: string) => requests.get<hall>(`/hall/${id}`),
    createHall: (hall: hall) => requests.post<void>('/hall', hall),
    updateHall: (hall: hall) => requests.put<void>(`/hall/`, hall),
    deleteHall: (id: string) => requests.delete<void>(`/hall/${id}`),
}
const Movies = {
    getMovies: () => requests.get<movie[]>('/movie'),
    getMovieById: (id: string) => requests.get<movie>(`/movie/${id}`),
    createMovie: (movie: movie) => requests.post<movie>(`/movie`,movie),
    updateMovie: (movie: movie) => requests.put<void>(`/movie`, movie),
    deleteMovie: (id: string) => requests.delete<void>(`/movie/${id}`),
    uploadImage: (id: string, file: FormData) => requests.put<void>(`/movie/${id}`, file)
};

const Sessions = {
    getSessions: () => requests.get<session[]>('/session'),
    getSessionById: (id: string) => requests.get<session>(`/session/${id}`),
    createSession: (session: session) => requests.post<void>('/session', session),
    updateSession: (session: session) => requests.put<void>('/session', session),
    deleteSession: (id: string) => requests.delete<void>(`/session/${id}`),
}
const Tickets = {
    getTickets: () => requests.get<ticket[]>('/ticket'),
    getTicketById: (id: string) => requests.get<ticket>(`/ticket/${id}`),
    getTicketByUserId: (id: string) => requests.get<ticket[]>(`/ticket/user/${id}`),
    createTicket: (ticket: ticket) => requests.post<void>('/ticket', ticket),
    updateTicket: (ticket: ticket) => requests.put<void>('/ticket', ticket),
    deleteTicket: (id: string) => requests.delete<void>(`/ticket/${id}`),
    getTicketsBySessionId: (id: string) => requests.get<ticket[]>(`/ticket/session/${id}`),
}
const agent = {
    Auth,
    Users,
    Cinemas,
    Halls,
    Movies,
    Sessions,
    Tickets,

};

export default agent;
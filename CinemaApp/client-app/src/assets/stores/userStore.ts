import { makeAutoObservable } from "mobx";
import ticket from "../models/ticket";
import user from "../models/user";
import LoginModel from "../models/loginModel";
import agent from "../API/agent";

export default class UserStore  {
    constructor() {
        makeAutoObservable(this);
        const login = localStorage.getItem('isLoggedIn');
        if (login && login==="true") {
            const userData = JSON.parse(localStorage.getItem('user')!);
            const ticketsData = JSON.parse(localStorage.getItem('tickets')!);
            console.log("userData:", userData); 
            console.log("ticketsData:", ticketsData);
            if (userData) {
                this.setUser(userData);  
            }

            if (ticketsData) {
                this.setTickets(JSON.parse(ticketsData));  
            }
        }
    }
    user: user | null = null;
    isLoggedIn = false;
    loading = false;
    tickets: ticket[] = [];

    getIsLoggedIn = () => this.isLoggedIn;
    setIsLoggedIn = (value: boolean) => {
        this.isLoggedIn= value;
    }
    setUser = (user: user | null) => {
        this.user = user;
    }
    getUser = () => {
        return this.user;
    }
    setLoading = (value: boolean) => {
        this.loading = value;
    }
    getLoading = () => {
        return this.loading;
    }
    setTickets = (tickets: ticket[]) => {
        this.tickets = tickets;
    }
    getTickets = () => {
        return this.tickets;
    }
    logIn = async (loginModel: LoginModel) => {
        this.setLoading(true);
        try {
            const LoginResponse = await agent.Auth.login(loginModel);
            if (LoginResponse.user) {
                console.log("Login response:", LoginResponse); 
                console.log("User response:", LoginResponse.user);
                this.setUser(LoginResponse.user);
                this.setIsLoggedIn(true);
                localStorage.setItem('user', JSON.stringify(LoginResponse.user));
                localStorage.setItem('isLoggedIn', 'true');
                this.getTicketsAsync();
            }
        }
        catch (error) {
            console.log("error while loggin in ", error);
        }
        finally {
            this.setLoading(false);
        }
    }
    logOut = () => {
        if (this.isLoggedIn) {
            this.setUser(null);
            this.setIsLoggedIn(false);
            localStorage.removeItem('user');
            localStorage.removeItem('isLoggedIn');
            localStorage.removeItem('tickets');
            return "successfully logged out";
        }
        else return "no user logged in";
    }
    getTicketsAsync = async () => {
        this.setLoading(true);
        try {
            this.setTickets(await agent.Tickets.getTicketByUserId(this.getUser()!.id));
            localStorage.setItem('tickets', JSON.stringify(this.getTickets()));
        }
        catch (error) {
            console.log("error while getting tickets ", error);
        }
        finally {
            this.setLoading(false);
        }
    }
}
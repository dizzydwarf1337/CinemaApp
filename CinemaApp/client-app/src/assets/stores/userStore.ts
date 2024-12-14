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
            console.log("userData:", userData); 
            this.setIsLoggedIn(login);
            if (userData) {
                this.setUser(userData);  
                this.getTicketsAsync();
            }

        }
    }
    user: user | null = null;
    isLoggedIn = false;
    loading = false;
    tickets: ticket[] = [];
    userLogOutSnack = false;
    userLogInSnack = false;
    refundSnack = false;
    purchaseSnack = false;
    setPurchaseSnack = (value: boolean) => {
        this.purchaseSnack = value;
    }
    getPurchaseSnack = () => this.purchaseSnack;
    setRefundSnack = (value: boolean) => {
        this.refundSnack = value;
    }
    getRefundSnack = () => this.refundSnack;
    setUserLogInSnack = (value: boolean) => {
        this.userLogInSnack = value;
    }
    getUserLogInSnack = () => this.userLogInSnack;
    setUserLogOutSnack = (value: boolean) => {
        this.userLogOutSnack = value;
    }
    getUserLogOutSnack = () => this.userLogOutSnack;
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
                this.setUserLogInSnack(true);
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
            this.setUserLogOutSnack(true);
        }
        else return "no user logged in";
    }
    getTicketsAsync = async () => {
        this.setLoading(true);
        agent.Tickets.getTicketByUserId(this.getUser()!.id).then((tickets) => { this.setTickets(tickets); }).catch((error) => { console.log("error while getting tickets: ", error); }).finally(() => this.setLoading(false));
    }
    addTicket = async (ticket: ticket) => {
        this.setLoading(true);
        this.setPurchaseSnack(true);
        try {
            await agent.Tickets.createTicket(ticket);
            await this.getTicketsAsync();
        }
        catch (error) {
            console.log("error while adding ticket ", error);
        }
        finally {
            this.setLoading(false);
        }
    }
    deleteTicket = async (id: string) => {
        this.setLoading(true);
        this.setRefundSnack(true);
        try {
            await agent.Tickets.deleteTicket(id);
            this.setTickets(this.tickets.filter(x => x.id !== id));
        }
        catch (error) {
            console.log("error while deleting ticket ", error);
        }
        finally {
            this.setLoading(false);
        }
    }
    createUser = async (user: user) => {
        this.setLoading(true);
        try {
            agent.Users.createUser(user).then(async () => await this.logIn({ email: user.email, password: user.password }));

        }
        catch (error) {
            console.log("error while registration", error);
        }
        finally {
            this.setLoading(false);
        }
    }
}
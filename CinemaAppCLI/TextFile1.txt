using Microsoft.EntityFrameworkCore;
using CinemaApp.Db;
using CinemaApp.Controllers;
using CinemaApp.Models.Dtos;
using CinemaApp.Models;
using Microsoft.AspNetCore.Mvc;
using Spectre.Console;
using System.Runtime.InteropServices;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

class Program
{
        // Importowanie funkcji do maksymalizacji okna konsoli
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

    private const int SW_MAXIMIZE = 3;

    static async Task Main(string[] args)
    {
        IntPtr handle = GetConsoleWindow();
        ShowWindow(handle, SW_MAXIMIZE);

        // Wyświetlenie animacji ASCII
        DisplayAsciiArt("ascii-art.txt");

        // Wyczekanie na naciśnięcie dowolnego klawisza
        AnsiConsole.MarkupLine("[bold yellow]Naciśnij dowolny klawisz, aby przejść do menu...[/]");
        Console.ReadKey();

        // Tworzenie opcji dla ApplicationContext
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CinemaApp;Trusted_Connection=True;MultipleActiveResultSets=true");
        var context = new ApplicationContext(optionsBuilder.Options);
        User user = new User{ Role=" "};
        bool exit = false;
        while (!exit)
        {
            string[] menuChoices;
            ShowHeader();
            if (user.Role == "admin") {
                menuChoices = new string[]{ "1. Show Movies", "2. Add Movie", "3. Edit Movie", "4. Delete Movie",
                    "5. Show tickets","6. Logout","9. Show Sessions", "10. Add Session", "11. Edit Session", "12. Delete Session",
                    "13. Show Cinemas", "14. Add Cinema", "15. Edit Cinema", "16. Delete Cinema",
                    "17. Show Halls", "18. Add Hall", "19. Edit Hall", "20. Delete Hall",
                    "21. Login User", "22. Show Users", "23. Add User", "24. Edit User", "25. Delete User", "26. Exit"};
                }
            else if (user.Role == "user") {
                    menuChoices = new string[] {
                        "1. Show Movies",
                        "2. Show User Tickets",
                        "3. Book Ticket",
                        "4. Show Sessions",
                        "5. Exit",
                        "6. Logout",
                    };
                }
            else
            {
              menuChoices = new string[] {
                        "1. Show Movies",
                        "2. Show Sessions",
                        "3. Login User",
                        "4. Add user",
                        "5. Exit"
                    };
            }

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold yellow]Wybierz opcję z menu poniżej:[/]")
                    .PageSize(15)
                    .AddChoices(menuChoices));

            Console.Clear();
            ShowHeader();

            switch (choice)
            {
                case "1. Show Movies": await ShowMovies(context);break;
                case "2. Add Movie": await AddMovie(context); break;
                case "3. Edit Movie": await EditMovie(context);break;
                case "4. Delete Movie": await DeleteMovie(context);break;
                case "5. Show tickets": await ShowUserTickets(context,user); break;
                case "9. Show Sessions": await ShowSessions(context); break;
                case "10. Add Session": await AddSession(context); break;
                case "11. Edit Session": await EditSession(context); break;
                case "12. Delete Session": await DeleteSession(context); break;
                case "13. Show Cinemas": await ShowCinemas(context); break;
                case "14. Add Cinema": await AddCinema(context); break;
                case "15. Edit Cinema": await EditCinema(context); break;
                case "16. Delete Cinema": await DeleteCinema(context); break;
                case "17. Show Halls": await ShowHalls(context); break;
                case "18. Add Hall": await AddHall(context); break;
                case "19. Edit Hall": await EditHall(context); break;
                case "20. Delete Hall": await DeleteHall(context); break;
                case  "22. Show Users": await ShowUsers(context); break;
                case "23. Add User": await AddUser(context); break;
                case "24. Edit User": await EditUser(context); break;
                case "25. Delete User": await DeleteUser(context); break;
                case "26. Exit": exit = true; break;
                case "2. Show User Tickets": await ShowUserTickets(context, user); break;
                case "3. Book Ticket": await BookTicket(context); break;
                case "4. Show Sessions": await ShowSessions(context); break;
                case "5. Exit": exit = true; break;
                case "2. Show Sessions": await ShowSessions(context); break;
                case "3. Login User": user = await LoginUser(context); break;
                case "4. Add user": await AddUser(context); break;
                case "6. Logout": user = new User { Role = " " }; break;
            }

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]\nNaciśnij dowolny klawisz, aby wrócić do menu...[/]");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    static void DisplayAsciiArt(string path)
    {
        try
        {
            string asciiArt = File.ReadAllText(path);
            AnsiConsole.Write(new Panel(asciiArt).Border(BoxBorder.Double).BorderStyle(Style.Parse("cyan")).Header("[bold yellow]Witamy![/]"));
            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Błąd podczas odczytu pliku ASCII: [/]" + ex.Message);
        }
    }

    static void ShowHeader()
    {
        AnsiConsole.Write(
            new FigletText("CinemaApp")
                .Centered()
                .Color(Color.Aqua));
        AnsiConsole.Markup("[bold yellow]Wybierz opcję z menu poniżej:[/]\n");
        AnsiConsole.Write(new Rule("[yellow]Menu[/]").RuleStyle("green"));
    }

    //movie
    static async Task ShowMovies(ApplicationContext context)
    {
        var movieController = new MovieController(context);
        var movies = await movieController.GetMoviesAsync();

        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[u]Title[/]");
        table.AddColumn("[u]Genre[/]");
        table.AddColumn("[u]Director[/]");
        table.AddColumn("[u]Duration[/]");

        foreach (var movie in movies)
        {
            table.AddRow(movie.Title, movie.Genre, movie.Director, movie.Duration.ToString());
        }

        AnsiConsole.Write(table);
    }

    static async Task AddMovie(ApplicationContext context)
    {
        string title = AnsiConsole.Ask<string>("[green]Podaj tytuł:[/]");
        string description = AnsiConsole.Ask<string>("[green]Podaj opis:[/]");
        string genre = AnsiConsole.Ask<string>("[green]Podaj gatunek:[/]");
        string director = AnsiConsole.Ask<string>("[green]Podaj reżysera:[/]");
        TimeOnly duration = AnsiConsole.Ask<TimeOnly>("[green]Podaj czas trwania (HH:MM):[/]");

        var movieDto = new movieDto
        {
            Title = title,
            Description = description,
            Genre = genre,
            Director = director,
            Duration = duration
        };

        var movieController = new MovieController(context);
        await movieController.CreateMovieAsync(movieDto);
        AnsiConsole.MarkupLine("[bold green]Film został pomyślnie dodany![/]");
    }

    static async Task EditMovie(ApplicationContext context)
    {
        var movieController = new MovieController(context);
        var movies = await movieController.GetMoviesAsync();

        if (movies.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych filmów do edycji.[/]");
            return;
        }

        // Wyświetlenie listy filmów z ich ID
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[u]ID[/]");
        table.AddColumn("[u]Title[/]");
        table.AddColumn("[u]Genre[/]");
        table.AddColumn("[u]Director[/]");

        foreach (var movie in movies)
        {
            table.AddRow(movie.Id.ToString(), movie.Title, movie.Genre, movie.Director);
        }

        AnsiConsole.Write(table);

        // Pobranie ID filmu do edycji
        var movieId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID filmu do edycji:[/]")
                .AddChoices(movies.Select(m => m.Id))
        );

        // Pobranie nowych danych filmu od użytkownika
        string title = AnsiConsole.Ask<string>("[green]Podaj nowy tytuł:[/]");
        string description = AnsiConsole.Ask<string>("[green]Podaj nowy opis:[/]");
        string genre = AnsiConsole.Ask<string>("[green]Podaj nowy gatunek:[/]");
        string director = AnsiConsole.Ask<string>("[green]Podaj nowego reżysera:[/]");
        TimeOnly duration = AnsiConsole.Ask<TimeOnly>("[green]Podaj nowy czas trwania (HH:MM):[/]");

        // Utworzenie obiektu DTO z nowymi danymi
        var movieDto = new movieDto
        {
            Id = movieId,
            Title = title,
            Description = description,
            Genre = genre,
            Director = director,
            Duration = duration
        };

        // Aktualizacja filmu
        await movieController.UpdateMovieAsync(movieDto);
        AnsiConsole.MarkupLine("[bold green]Film został pomyślnie zaktualizowany![/]");
    }

    static async Task DeleteMovie(ApplicationContext context)
    {
        var movieController = new MovieController(context);
        var movies = await movieController.GetMoviesAsync();

        if (movies.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych filmów do usunięcia.[/]");
            return;
        }

        // Wyświetlenie listy filmów z ich ID
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[u]ID[/]");
        table.AddColumn("[u]Title[/]");
        table.AddColumn("[u]Genre[/]");
        table.AddColumn("[u]Director[/]");

        foreach (var movie in movies)
        {
            table.AddRow(movie.Id.ToString(), movie.Title, movie.Genre, movie.Director);
        }

        AnsiConsole.Write(table);

        // Pobranie ID filmu do usunięcia
        var movieId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID filmu do usunięcia:[/]")
                .AddChoices(movies.Select(m => m.Id))
        );

        // Usunięcie filmu
        await movieController.DeleteMovieByIdAsync(movieId);
        AnsiConsole.MarkupLine("[bold red]Film został pomyślnie usunięty![/]");
    }
    //tickets

    static async Task ShowUserTickets(ApplicationContext context, User user)
    {
        if (user.Role == "admin")
        {
            var userController = new UserController(context);
            var users = await userController.GetUserAsync();

            if (users == null || users.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Brak dostępnych użytkowników.[/]");
                return;
            }

            // Wyświetlenie tabeli z użytkownikami
            var userTable = new Table().Border(TableBorder.Rounded);
            userTable.AddColumn("[u]ID[/]");
            userTable.AddColumn("[u]Imię[/]");
            userTable.AddColumn("[u]Nazwisko[/]");
            userTable.AddColumn("[u]Email[/]");

            foreach (var user1 in users)
            {
                userTable.AddRow(user1.Id.ToString(), user1.Name, user1.LastName, user1.Email);
            }

            AnsiConsole.Write(userTable);

            // Wybór użytkownika na podstawie ID
            var userId = AnsiConsole.Prompt(
                new SelectionPrompt<Guid>()
                    .Title("[green]Wybierz ID użytkownika, aby wyświetlić bilety:[/]")
                    .AddChoices(users.Select(u => u.Id))
            );

            var ticketController = new TicketController(context);
            var tickets = await ticketController.GetTicketsByUserIdAsync(userId);

            if (tickets == null || tickets.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Brak dostępnych biletów dla tego użytkownika.[/]");
                return;
            }

            // Wyświetlenie tabeli z biletami
            var ticketTable = new Table().Border(TableBorder.Rounded);
            ticketTable.AddColumn("[u]ID[/]");
            ticketTable.AddColumn("[u]Seat[/]");
            ticketTable.AddColumn("[u]Status[/]");
            ticketTable.AddColumn("[u]Price[/]");

            foreach (var ticket in tickets)
            {
                ticketTable.AddRow(ticket.Id.ToString(), ticket.Seat, ticket.Status, ticket.Price.ToString("C"));
            }

            AnsiConsole.Write(ticketTable);
        }
        else if (user.Role == "user")
        {
            var ticketController = new TicketController(context);
            var tickets = await ticketController.GetTicketsByUserIdAsync(user.Id);

            if (tickets == null || tickets.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]Brak dostępnych biletów dla tego użytkownika.[/]");
                return;
            }

            var sessionIds = tickets.Select(t => t.SessionId).Distinct();
            var sessions = await context.Sessions
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Where(s => sessionIds.Contains(s.Id))
                .ToListAsync();

            var ticketTable = new Table().Border(TableBorder.Rounded);
            ticketTable.AddColumn("[u]Movie[/]");
            ticketTable.AddColumn("[u]Hall[/]");
            ticketTable.AddColumn("[u]Date and time[/]");
            ticketTable.AddColumn("[u]Seat[/]");
            ticketTable.AddColumn("[u]Status[/]");
            ticketTable.AddColumn("[u]Price[/]");

            foreach (var ticket in tickets)
            {
                var session = sessions.FirstOrDefault(s => s.Id == ticket.SessionId);
                if (session != null)
                {
                    ticketTable.AddRow(
                        session.Movie.Title,
                        session.Hall.Number.ToString(),
                        session.Date.ToString("yyyy-MM-dd HH:mm"),
                        ticket.Seat.ToString(),
                        ticket.Status,
                        ticket.Price.ToString("C")
                    );
                }
            }

            AnsiConsole.Write(ticketTable);
        }
    }

    static async Task BookTicket(ApplicationContext context)
    {
        // Pobranie listy użytkowników
        var userController = new UserController(context);
        var users = await userController.GetUserAsync();

        if (users == null || users.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych użytkowników.[/]");
            return;
        }

        // Wyświetlenie tabeli z użytkownikami
        var userTable = new Table().Border(TableBorder.Rounded);
        userTable.AddColumn("[u]ID[/]");
        userTable.AddColumn("[u]Imię[/]");
        userTable.AddColumn("[u]Nazwisko[/]");
        userTable.AddColumn("[u]Email[/]");

        foreach (var user in users)
        {
            userTable.AddRow(user.Id.ToString(), user.Name, user.LastName, user.Email);
        }

        AnsiConsole.Write(userTable);

        // Wybór użytkownika na podstawie ID
        var userId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID użytkownika, aby dodać bilet:[/]")
                .AddChoices(users.Select(u => u.Id))
        );

        // Pobranie listy sesji
        var sessionController = new SessionController(context);
        var sessions = await sessionController.GetSessionAsync();

        if (sessions == null || sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych sesji.[/]");
            return;
        }

        // Wyświetlenie tabeli z sesjami
        var sessionTable = new Table().Border(TableBorder.Rounded);
        sessionTable.AddColumn("[u]ID[/]");
        sessionTable.AddColumn("[u]Film ID[/]");
        sessionTable.AddColumn("[u]Data[/]");
        sessionTable.AddColumn("[u]Cena biletu[/]");
        sessionTable.AddColumn("[u]Dostępne miejsca[/]");

        foreach (var session in sessions)
        {
            sessionTable.AddRow(session.Id.ToString(), session.MovieId.ToString(), session.Date.ToString("yyyy-MM-dd HH:mm"), session.TicketPrice.ToString("C"), session.AvailibleSeats.ToString());
        }

        AnsiConsole.Write(sessionTable);

        // Wybór sesji na podstawie ID
        var sessionId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID sesji, aby zarezerwować bilet:[/]")
                .AddChoices(sessions.Select(s => s.Id))
        );

        Console.Write("Wprowadź ilość miejsc: ");
        if (!int.TryParse(Console.ReadLine(), out int numberOfSeats) || numberOfSeats <= 0)
        {
            AnsiConsole.MarkupLine("[red]Niepoprawna ilość miejsc. Spróbuj ponownie.[/]");
            return;
        }

        // Zmiana nazwy zmiennej na `selectedSession` dla uniknięcia konfliktu
        var selectedSession = sessions.FirstOrDefault(s => s.Id == sessionId);

        if (selectedSession == null || selectedSession.AvailibleSeats < numberOfSeats)
        {
            AnsiConsole.MarkupLine("[red]Niewystarczająca ilość dostępnych miejsc.[/]");
            return;
        }

        // Utworzenie obiektu biletu
        var ticketDto = new ticketDto
        {
            SessionId = sessionId,
            NumberOfSeats = numberOfSeats,
            Seat = "A1", // Przykładowe miejsce, tutaj można wprowadzić logikę wyboru miejsca
            Status = "Reserved",
            Price = selectedSession.TicketPrice * numberOfSeats,
            UserId = userId // Przypisanie wybranego ID użytkownika
        };

        var ticketController = new TicketController(context);
        await ticketController.CreateTicketAsync(ticketDto);
        AnsiConsole.MarkupLine("[bold green]Bilet został pomyślnie zarezerwowany dla wybranego użytkownika![/]");
    }

    static async Task EditTicket(ApplicationContext context)
    {
        var userController = new UserController(context);
        var users = await userController.GetUserAsync();

        if (users == null || users.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych użytkowników.[/]");
            return;
        }

        // Wyświetlenie tabeli z użytkownikami
        var userTable = new Table().Border(TableBorder.Rounded);
        userTable.AddColumn("[u]ID[/]");
        userTable.AddColumn("[u]Imię[/]");
        userTable.AddColumn("[u]Nazwisko[/]");
        userTable.AddColumn("[u]Email[/]");

        foreach (var user in users)
        {
            userTable.AddRow(user.Id.ToString(), user.Name, user.LastName, user.Email);
        }

        AnsiConsole.Write(userTable);

        var userId = AnsiConsole.Ask<Guid>("[green]Podaj ID użytkownika, aby pobrać jego bilety:[/]");
        var ticketController = new TicketController(context);
        var tickets = await ticketController.GetTicketsByUserIdAsync(userId);

        if (tickets == null || tickets.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych biletów do edycji dla tego użytkownika.[/]");
            return;
        }

        // Wyświetlenie tabeli z biletami
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[u]ID[/]");
        table.AddColumn("[u]Seat[/]");
        table.AddColumn("[u]Status[/]");
        table.AddColumn("[u]Price[/]");

        foreach (var ticket in tickets)
        {
            table.AddRow(ticket.Id.ToString(), ticket.Seat, ticket.Status, ticket.Price.ToString("C"));
        }

        AnsiConsole.Write(table);

        // Pobranie ID biletu do edycji
        var ticketId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID biletu do edycji:[/]")
                .AddChoices(tickets.Select(t => t.Id))
        );

        // Pobranie nowych danych od użytkownika
        string seat = AnsiConsole.Ask<string>("[green]Podaj nowe miejsce:[/]");
        string status = AnsiConsole.Ask<string>("[green]Podaj nowy status (np. 'Reserved', 'Cancelled'):[/]");
        double price = AnsiConsole.Ask<double>("[green]Podaj nową cenę biletu:[/]");
        int numberOfSeats = AnsiConsole.Ask<int>("[green]Podaj nową liczbę miejsc:[/]");

        var ticketDto = new ticketDto
        {
            Id = ticketId,
            Seat = seat,
            Status = status,
            Price = price,
            NumberOfSeats = numberOfSeats
        };

        await ticketController.UpdateTicketAsync(ticketDto);
        AnsiConsole.MarkupLine("[bold green]Bilet został pomyślnie zaktualizowany![/]");
    }

    static async Task DeleteTicket(ApplicationContext context)
    {
        var userController = new UserController(context);
        var users = await userController.GetUserAsync();

        if (users == null || users.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych użytkowników.[/]");
            return;
        }

        // Wyświetlenie tabeli z użytkownikami
        var userTable = new Table().Border(TableBorder.Rounded);
        userTable.AddColumn("[u]ID[/]");
        userTable.AddColumn("[u]Imię[/]");
        userTable.AddColumn("[u]Nazwisko[/]");
        userTable.AddColumn("[u]Email[/]");

        foreach (var user in users)
        {
            userTable.AddRow(user.Id.ToString(), user.Name, user.LastName, user.Email);
        }

        AnsiConsole.Write(userTable);

        var userId = AnsiConsole.Ask<Guid>("[green]Podaj ID użytkownika, aby pobrać jego bilety:[/]");
        var ticketController = new TicketController(context);
        var tickets = await ticketController.GetTicketsByUserIdAsync(userId);

        if (tickets == null || tickets.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych biletów do usunięcia dla tego użytkownika.[/]");
            return;
        }

        // Wyświetlenie tabeli z biletami
        var table = new Table().Border(TableBorder.Rounded);
        table.AddColumn("[u]ID[/]");
        table.AddColumn("[u]Seat[/]");
        table.AddColumn("[u]Status[/]");
        table.AddColumn("[u]Price[/]");

        foreach (var ticket in tickets)
        {
            table.AddRow(ticket.Id.ToString(), ticket.Seat, ticket.Status, ticket.Price.ToString("C"));
        }

        AnsiConsole.Write(table);

        // Pobranie ID biletu do usunięcia
        var ticketId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID biletu do usunięcia:[/]")
                .AddChoices(tickets.Select(t => t.Id))
        );

        await ticketController.DeleteTicketByIdAsync(ticketId);
        AnsiConsole.MarkupLine("[bold red]Bilet został pomyślnie usunięty![/]");
    }
    //session
    static async Task ShowSessions(ApplicationContext context)
    {
        var sessionController = new SessionController(context);
        var sessions = await sessionController.GetSessionAsync();

        if (sessions == null || sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych seansów.[/]");
            return;
        }

        // Wyświetlenie tabeli z seansami
        var sessionTable = new Table().Border(TableBorder.Rounded);
        sessionTable.AddColumn("[u]ID[/]");
        sessionTable.AddColumn("[u]Film ID[/]");
        sessionTable.AddColumn("[u]Sala ID[/]");
        sessionTable.AddColumn("[u]Data[/]");
        sessionTable.AddColumn("[u]Cena biletu[/]");

        foreach (var session in sessions)
        {
            sessionTable.AddRow(
                session.Id.ToString(),
                session.MovieId.ToString(),
                session.HallId.ToString(),
                session.Date.ToString("yyyy-MM-dd HH:mm"),
                session.TicketPrice.ToString("C")
            );
        }

        AnsiConsole.Write(sessionTable);
    }

    static async Task AddSession(ApplicationContext context)
    {
        // Wyświetlenie listy filmów
        var movieController = new MovieController(context);
        var movies = await movieController.GetMoviesAsync();

        if (movies == null || movies.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych filmów.[/]");
            return;
        }

        var movieTable = new Table().Border(TableBorder.Rounded);
        movieTable.AddColumn("[u]ID[/]");
        movieTable.AddColumn("[u]Tytuł[/]");
        movieTable.AddColumn("[u]Gatunek[/]");
        movieTable.AddColumn("[u]Reżyser[/]");

        foreach (var movie in movies)
        {
            movieTable.AddRow(movie.Id.ToString(), movie.Title, movie.Genre, movie.Director);
        }

        AnsiConsole.Write(movieTable);

        // Wybór filmu
        var movieId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID filmu:[/]")
                .AddChoices(movies.Select(m => m.Id))
        );

        // Wyświetlenie listy sal
        var hallController = new HallController(context);
        var halls = await hallController.GetHallsAsync();

        if (halls == null || halls.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych sal.[/]");
            return;
        }

        var hallTable = new Table().Border(TableBorder.Rounded);
        hallTable.AddColumn("[u]ID[/]");
        hallTable.AddColumn("[u]Numer[/]");
        hallTable.AddColumn("[u]Liczba miejsc[/]");

        foreach (var hall in halls)
        {
            hallTable.AddRow(hall.Id.ToString(), hall.Number, hall.Seats.ToString());
        }

        AnsiConsole.Write(hallTable);

        // Wybór sali
        var hallId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID sali:[/]")
                .AddChoices(halls.Select(h => h.Id))
        );

        // Pobranie danych sesji
        var sessionDate = AnsiConsole.Ask<DateTime>("[green]Podaj datę sesji (yyyy-MM-dd HH:mm):[/]");
        var ticketPrice = AnsiConsole.Ask<double>("[green]Podaj cenę biletu:[/]");
        var availableSeats = AnsiConsole.Ask<int>("[green]Podaj liczbę dostępnych miejsc:[/]");

        var sessionDto = new sessionDto
        {
            MovieId = movieId,
            HallId = hallId,
            Date = sessionDate,
            TicketPrice = ticketPrice,
            AvailibleSeats = availableSeats
        };

        var sessionController = new SessionController(context);
        await sessionController.CreateSessionAsync(sessionDto);
        AnsiConsole.MarkupLine("[bold green]Sesja została pomyślnie dodana![/]");
    }

    static async Task EditSession(ApplicationContext context)
    {
        // Wyświetlenie listy sesji
        var sessionController = new SessionController(context);
        var sessions = await sessionController.GetSessionAsync();

        if (sessions == null || sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych sesji do edycji.[/]");
            return;
        }

        var sessionTable = new Table().Border(TableBorder.Rounded);
        sessionTable.AddColumn("[u]ID[/]");
        sessionTable.AddColumn("[u]Film ID[/]");
        sessionTable.AddColumn("[u]Sala ID[/]");
        sessionTable.AddColumn("[u]Data[/]");
        sessionTable.AddColumn("[u]Cena biletu[/]");
        sessionTable.AddColumn("[u]Dostępne miejsca[/]");

        foreach (var session in sessions)
        {
            sessionTable.AddRow(
                session.Id.ToString(),
                session.MovieId.ToString(),
                session.HallId.ToString(),
                session.Date.ToString("yyyy-MM-dd HH:mm"),
                session.TicketPrice.ToString("C"),
                session.AvailibleSeats.ToString()
            );
        }

        AnsiConsole.Write(sessionTable);

        // Wybór sesji do edycji
        var sessionId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID sesji do edycji:[/]")
                .AddChoices(sessions.Select(s => s.Id))
        );

        // Pobranie nowych danych sesji
        var sessionDate = AnsiConsole.Ask<DateTime>("[green]Podaj nową datę sesji (yyyy-MM-dd HH:mm):[/]");
        var ticketPrice = AnsiConsole.Ask<double>("[green]Podaj nową cenę biletu:[/]");
        var availableSeats = AnsiConsole.Ask<int>("[green]Podaj nową liczbę dostępnych miejsc:[/]");

        var sessionDto = new sessionDto
        {
            Id = sessionId,
            Date = sessionDate,
            TicketPrice = ticketPrice,
            AvailibleSeats = availableSeats
        };

        await sessionController.UpdateSessionAsync(sessionDto);
        AnsiConsole.MarkupLine("[bold green]Sesja została pomyślnie zaktualizowana![/]");
    }

    static async Task DeleteSession(ApplicationContext context)
    {
        // Wyświetlenie listy sesji
        var sessionController = new SessionController(context);
        var sessions = await sessionController.GetSessionAsync();

        if (sessions == null || sessions.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych sesji do usunięcia.[/]");
            return;
        }

        var sessionTable = new Table().Border(TableBorder.Rounded);
        sessionTable.AddColumn("[u]ID[/]");
        sessionTable.AddColumn("[u]Film ID[/]");
        sessionTable.AddColumn("[u]Sala ID[/]");
        sessionTable.AddColumn("[u]Data[/]");
        sessionTable.AddColumn("[u]Cena biletu[/]");
        sessionTable.AddColumn("[u]Dostępne miejsca[/]");

        foreach (var session in sessions)
        {
            sessionTable.AddRow(
                session.Id.ToString(),
                session.MovieId.ToString(),
                session.HallId.ToString(),
                session.Date.ToString("yyyy-MM-dd HH:mm"),
                session.TicketPrice.ToString("C"),
                session.AvailibleSeats.ToString()
            );
        }

        AnsiConsole.Write(sessionTable);

        // Wybór sesji do usunięcia
        var sessionId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID sesji do usunięcia:[/]")
                .AddChoices(sessions.Select(s => s.Id))
        );

        await sessionController.DeleteSessionByIdAsync(sessionId);
        AnsiConsole.MarkupLine("[bold red]Sesja została pomyślnie usunięta![/]");
    }

    //cinemas
    static async Task ShowCinemas(ApplicationContext context)
    {
        var cinemaController = new CinemaController(context);
        var cinemas = await cinemaController.GetCinemasAsync();

        if (cinemas == null || cinemas.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych kin.[/]");
            return;
        }

        // Wyświetlenie tabeli z kinami
        var cinemaTable = new Table().Border(TableBorder.Rounded);
        cinemaTable.AddColumn("[u]ID[/]");
        cinemaTable.AddColumn("[u]Nazwa[/]");
        cinemaTable.AddColumn("[u]Adres[/]");

        foreach (var cinema in cinemas)
        {
            cinemaTable.AddRow(cinema.Id.ToString(), cinema.Name, cinema.Address);
        }

        AnsiConsole.Write(cinemaTable);
    }

    static async Task AddCinema(ApplicationContext context)
    {
        string name = AnsiConsole.Ask<string>("[green]Podaj nazwę kina:[/]");
        string address = AnsiConsole.Ask<string>("[green]Podaj adres kina:[/]");

        var cinemaDto = new cinemaDto { Name = name, Address = address };
        var cinemaController = new CinemaController(context);
        await cinemaController.CreateCinemaAsync(cinemaDto);
        AnsiConsole.MarkupLine("[bold green]Kino zostało pomyślnie dodane![/]");
    }

    static async Task EditCinema(ApplicationContext context)
    {
        var cinemaController = new CinemaController(context);
        var cinemas = await cinemaController.GetCinemasAsync();

        if (cinemas == null || cinemas.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych kin do edycji.[/]");
            return;
        }

        // Wyświetlenie tabeli z kinami
        var cinemaTable = new Table().Border(TableBorder.Rounded);
        cinemaTable.AddColumn("[u]ID[/]");
        cinemaTable.AddColumn("[u]Nazwa[/]");
        cinemaTable.AddColumn("[u]Adres[/]");

        foreach (var cinema in cinemas)
        {
            cinemaTable.AddRow(cinema.Id.ToString(), cinema.Name, cinema.Address);
        }

        AnsiConsole.Write(cinemaTable);

        // Wybór kina do edycji
        var cinemaId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID kina do edycji:[/]")
                .AddChoices(cinemas.Select(c => c.Id))
        );

        // Pobranie nowych danych dla kina
        string name = AnsiConsole.Ask<string>("[green]Podaj nową nazwę kina:[/]");
        string address = AnsiConsole.Ask<string>("[green]Podaj nowy adres kina:[/]");

        var cinemaDto = new cinemaDto { Id = cinemaId, Name = name, Address = address };
        await cinemaController.UpdateCinemaAsync(cinemaDto);
        AnsiConsole.MarkupLine("[bold green]Kino zostało pomyślnie zaktualizowane![/]");
    }

    static async Task DeleteCinema(ApplicationContext context)
    {
        var cinemaController = new CinemaController(context);
        var cinemas = await cinemaController.GetCinemasAsync();

        if (cinemas == null || cinemas.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych kin do usunięcia.[/]");
            return;
        }

        // Wyświetlenie tabeli z kinami
        var cinemaTable = new Table().Border(TableBorder.Rounded);
        cinemaTable.AddColumn("[u]ID[/]");
        cinemaTable.AddColumn("[u]Nazwa[/]");
        cinemaTable.AddColumn("[u]Adres[/]");

        foreach (var cinema in cinemas)
        {
            cinemaTable.AddRow(cinema.Id.ToString(), cinema.Name, cinema.Address);
        }

        AnsiConsole.Write(cinemaTable);

        // Wybór kina do usunięcia
        var cinemaId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID kina do usunięcia:[/]")
                .AddChoices(cinemas.Select(c => c.Id))
        );

        await cinemaController.DeleteCinemaAsync(cinemaId);
        AnsiConsole.MarkupLine("[bold red]Kino zostało pomyślnie usunięte![/]");
    }
    //halls

    static async Task ShowHalls(ApplicationContext context)
    {
        var hallController = new HallController(context);
        var halls = await hallController.GetHallsAsync();

        if (halls == null || halls.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych sal.[/]");
            return;
        }

        // Wyświetlenie tabeli z salami
        var hallTable = new Table().Border(TableBorder.Rounded);
        hallTable.AddColumn("[u]ID[/]");
        hallTable.AddColumn("[u]Numer sali[/]");
        hallTable.AddColumn("[u]Liczba miejsc[/]");
        hallTable.AddColumn("[u]Kino ID[/]");

        foreach (var hall in halls)
        {
            hallTable.AddRow(hall.Id.ToString(), hall.Number, hall.Seats.ToString(), hall.CinemaId.ToString());
        }

        AnsiConsole.Write(hallTable);
    }

    static async Task AddHall(ApplicationContext context)
    {
        // Wyświetlenie listy kin dla ułatwienia wyboru
        var cinemaController = new CinemaController(context);
        var cinemas = await cinemaController.GetCinemasAsync();

        if (cinemas == null || cinemas.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych kin.[/]");
            return;
        }

        var cinemaTable = new Table().Border(TableBorder.Rounded);
        cinemaTable.AddColumn("[u]ID[/]");
        cinemaTable.AddColumn("[u]Nazwa[/]");
        cinemaTable.AddColumn("[u]Adres[/]");

        foreach (var cinema in cinemas)
        {
            cinemaTable.AddRow(cinema.Id.ToString(), cinema.Name, cinema.Address);
        }

        AnsiConsole.Write(cinemaTable);

        // Pobranie informacji o nowej sali
        string number = AnsiConsole.Ask<string>("[green]Podaj nowy numer sali:[/]");
        int seats = AnsiConsole.Ask<int>("[green]Podaj liczbę miejsc w sali:[/]");

        // Wybór ID kina
        var cinemaId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID kina, do którego należy sala:[/]")
                .AddChoices(cinemas.Select(c => c.Id))
        );

        var hallDto = new hallDto { Number = number, Seats = seats, CinemaId = cinemaId };
        var hallController = new HallController(context);
        await hallController.CreateHallAsync(hallDto);
        AnsiConsole.MarkupLine("[bold green]Sala została pomyślnie dodana![/]");
    }

    static async Task EditHall(ApplicationContext context)
    {
        var hallController = new HallController(context);
        var halls = await hallController.GetHallsAsync();

        if (halls == null || halls.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych sal do edycji.[/]");
            return;
        }

        // Wyświetlenie tabeli z salami
        var hallTable = new Table().Border(TableBorder.Rounded);
        hallTable.AddColumn("[u]ID[/]");
        hallTable.AddColumn("[u]Numer sali[/]");
        hallTable.AddColumn("[u]Liczba miejsc[/]");
        hallTable.AddColumn("[u]Kino ID[/]");

        foreach (var hall in halls)
        {
            hallTable.AddRow(hall.Id.ToString(), hall.Number, hall.Seats.ToString(), hall.CinemaId.ToString());
        }

        AnsiConsole.Write(hallTable);

        // Wybór ID sali do edycji
        var hallId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID sali do edycji:[/]")
                .AddChoices(halls.Select(h => h.Id))
        );

        // Pobranie nowych informacji o sali
        string number = AnsiConsole.Ask<string>("[green]Podaj nowy numer sali:[/]");
        int seats = AnsiConsole.Ask<int>("[green]Podaj nową liczbę miejsc:[/]");

        // Wybór ID kina
        var cinemaController = new CinemaController(context);
        var cinemas = await cinemaController.GetCinemasAsync();
        if (cinemas == null || cinemas.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych kin.[/]");
            return;
        }

        var cinemaId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz nowe ID kina, do którego należy sala:[/]")
                .AddChoices(cinemas.Select(c => c.Id))
        );

        var hallDto = new hallDto { Id = hallId, Number = number, Seats = seats, CinemaId = cinemaId };
        await hallController.UpdateHallAsync(hallDto);
        AnsiConsole.MarkupLine("[bold green]Sala została pomyślnie zaktualizowana![/]");
    }

    static async Task DeleteHall(ApplicationContext context)
    {
        var hallController = new HallController(context);
        var halls = await hallController.GetHallsAsync();

        if (halls == null || halls.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych sal do usunięcia.[/]");
            return;
        }

        // Wyświetlenie tabeli z salami
        var hallTable = new Table().Border(TableBorder.Rounded);
        hallTable.AddColumn("[u]ID[/]");
        hallTable.AddColumn("[u]Numer sali[/]");
        hallTable.AddColumn("[u]Liczba miejsc[/]");
        hallTable.AddColumn("[u]Kino ID[/]");

        foreach (var hall in halls)
        {
            hallTable.AddRow(hall.Id.ToString(), hall.Number, hall.Seats.ToString(), hall.CinemaId.ToString());
        }

        AnsiConsole.Write(hallTable);

        // Wybór ID sali do usunięcia
        var hallId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID sali do usunięcia:[/]")
                .AddChoices(halls.Select(h => h.Id))
        );

        await hallController.DeleteHallAsync(hallId);
        AnsiConsole.MarkupLine("[bold red]Sala została pomyślnie usunięta![/]");
    }
    //users
    static async Task<User> LoginUser(ApplicationContext context)
    {
        string email = AnsiConsole.Ask<string>("[green]Wprowadź email:[/]");
        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Wprowadź hasło:[/]")
                .PromptStyle("red")
                .Secret()
        );

        var loginController = new LoginController(context);
        var result = await loginController.Login(new Login { Email = email, Password = password });

        if (result is OkObjectResult okResult)
        {
            var response = JObject.FromObject(okResult.Value);
            var userResponse = response["User"]?.ToObject<User>();
            return userResponse;
        }
        else
        {
            AnsiConsole.MarkupLine("[bold red]Niepoprawne dane logowania.[/]");
            return null;
        }
    }

    static async Task ShowUsers(ApplicationContext context)
    {
        var userController = new UserController(context);
        var users = await userController.GetUserAsync();

        if (users == null || users.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych użytkowników.[/]");
            return;
        }

        // Wyświetlenie tabeli z użytkownikami
        var userTable = new Table().Border(TableBorder.Rounded);
        userTable.AddColumn("[u]ID[/]");
        userTable.AddColumn("[u]Imię[/]");
        userTable.AddColumn("[u]Nazwisko[/]");
        userTable.AddColumn("[u]Email[/]");

        foreach (var user in users)
        {
            userTable.AddRow(user.Id.ToString(), user.Name, user.LastName, user.Email);
        }

        AnsiConsole.Write(userTable);
    }

    static async Task AddUser(ApplicationContext context)
    {
        string name = AnsiConsole.Ask<string>("[green]Podaj imię:[/]");
        string lastName = AnsiConsole.Ask<string>("[green]Podaj nazwisko:[/]");
        string email = AnsiConsole.Ask<string>("[green]Podaj email:[/]");
        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Podaj hasło:[/]")
                .PromptStyle("red")
                .Secret()
        );

        var userDto = new userDto { Name = name, LastName = lastName, Email = email, Password = password };
        var userController = new UserController(context);
        await userController.CreateUserAsync(userDto);
        AnsiConsole.MarkupLine("[bold green]Użytkownik został pomyślnie dodany![/]");
    }

    static async Task EditUser(ApplicationContext context)
    {
        var userController = new UserController(context);
        var users = await userController.GetUserAsync();

        if (users == null || users.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych użytkowników do edycji.[/]");
            return;
        }

        // Wyświetlenie tabeli z użytkownikami
        var userTable = new Table().Border(TableBorder.Rounded);
        userTable.AddColumn("[u]ID[/]");
        userTable.AddColumn("[u]Imię[/]");
        userTable.AddColumn("[u]Nazwisko[/]");
        userTable.AddColumn("[u]Email[/]");

        foreach (var user in users)
        {
            userTable.AddRow(user.Id.ToString(), user.Name, user.LastName, user.Email);
        }

        AnsiConsole.Write(userTable);

        // Wybór użytkownika do edycji
        var userId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID użytkownika do edycji:[/]")
                .AddChoices(users.Select(u => u.Id))
        );

        // Pobranie nowych danych
        string name = AnsiConsole.Ask<string>("[green]Podaj nowe imię:[/]");
        string lastName = AnsiConsole.Ask<string>("[green]Podaj nowe nazwisko:[/]");
        string email = AnsiConsole.Ask<string>("[green]Podaj nowy email:[/]");
        string password = AnsiConsole.Prompt(
            new TextPrompt<string>("[green]Podaj nowe hasło:[/]")
                .PromptStyle("red")
                .Secret()
        );

        var userDto = new userDto { Id = userId, Name = name, LastName = lastName, Email = email, Password = password };
        await userController.UpdateUserAsync(userDto);
        AnsiConsole.MarkupLine("[bold green]Dane użytkownika zostały pomyślnie zaktualizowane![/]");
    }

    static async Task DeleteUser(ApplicationContext context)
    {
        var userController = new UserController(context);
        var users = await userController.GetUserAsync();

        if (users == null || users.Count == 0)
        {
            AnsiConsole.MarkupLine("[red]Brak dostępnych użytkowników do usunięcia.[/]");
            return;
        }

        // Wyświetlenie tabeli z użytkownikami
        var userTable = new Table().Border(TableBorder.Rounded);
        userTable.AddColumn("[u]ID[/]");
        userTable.AddColumn("[u]Imię[/]");
        userTable.AddColumn("[u]Nazwisko[/]");
        userTable.AddColumn("[u]Email[/]");

        foreach (var user in users)
        {
            userTable.AddRow(user.Id.ToString(), user.Name, user.LastName, user.Email);
        }

        AnsiConsole.Write(userTable);

        // Wybór użytkownika do usunięcia
        var userId = AnsiConsole.Prompt(
            new SelectionPrompt<Guid>()
                .Title("[green]Wybierz ID użytkownika do usunięcia:[/]")
                .AddChoices(users.Select(u => u.Id))
        );

        await userController.DeleteUserByIdAsync(userId);
        AnsiConsole.MarkupLine("[bold red]Użytkownik został pomyślnie usunięty![/]");
    }

}

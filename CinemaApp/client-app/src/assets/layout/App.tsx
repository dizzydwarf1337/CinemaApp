import { ThemeProvider } from "@emotion/react"
import { Outlet } from "react-router-dom"
import theme from "../theme/theme"
import NavBar from "./NavBar"
import "./styles.css"

export default function App() {


  return (
      <>
          <ThemeProvider theme={theme}>
              <NavBar />
              <Outlet />
        </ThemeProvider>
    </>
  )
}


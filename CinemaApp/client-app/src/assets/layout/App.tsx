import { ThemeProvider } from "@emotion/react"
import { Outlet } from "react-router-dom"
import theme from "../theme/theme"
import NavBar from "./NavBar"
import "./styles.css"
import { Box } from "@mui/material"

export default function App() {


  return (
      <>
          <ThemeProvider theme={theme}>
              <Box sx={{mb:"100px"} }>
                  <NavBar />
              </Box>
              <Outlet />
        </ThemeProvider>
    </>
  )
}


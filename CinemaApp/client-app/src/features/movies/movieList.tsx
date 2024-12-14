import { Box } from "@mui/material";
import { useStore } from "../../assets/stores/store";
import { observer } from "mobx-react-lite";
import MovieListItem from "./movieListItem";

export default observer(function MovieList() {
    const { cinemaStore } = useStore();
;    return (
    <>
        <Box display="grid" gridTemplateColumns="auto auto auto auto" rowGap="30px" columnGap="20px">
                {cinemaStore.movies.map(movie => (
                    <Box key={movie.id}>
                        <MovieListItem movie={movie} />
                    </Box>
                ))}
            </Box>
        </>
    )
})
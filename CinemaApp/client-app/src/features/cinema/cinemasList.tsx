import { observer } from "mobx-react-lite"
import { useStore } from "../../assets/stores/store";
import { Box } from "@mui/material";
import CinemaItemList2 from "./cinemaItemList2";



export default observer(function cinemasList() {
    const { cinemaStore } = useStore();
    return (
        <>
            <Box display="grid" gridTemplateColumns="auto auto auto auto" gap="10px" rowGap="50px">
                {cinemaStore.cinemas.map(cinema => (
                    <CinemaItemList2 key={cinema.id} cinema={cinema} />
                ))}
            </Box>
        </>
    )
})
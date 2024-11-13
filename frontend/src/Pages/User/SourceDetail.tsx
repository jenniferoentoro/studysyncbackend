import  { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import NavbarInside from "../../Components/NavbarInside.tsx";
import SourceAPI from "../../APIs/SourceAPI.tsx";
import Container from 'react-bootstrap/Container';
import './SourceDetail.css';
interface Source {
    id: number;
    title: string;
    description: string;
    createdOn: string; // Assuming "createdOn" field represents the date
    urlFile: string;
    fileName: string;
    category: {
        id: number;
        name: string;
    };
    user: {
        id: number;
        email: string;
    };
}

function SourceDetail(){
    const { id } = useParams<{ id: string }>();
    const [sourceItem, setSourceItem] = useState<Source | null>(null);

    useEffect(() => {
        async function fetchSourceItem() {
            try {
                if (!id) return;
                const sourceResponse = await SourceAPI.getSourceById(parseInt(id)); // Convert id to number
                setSourceItem(sourceResponse);
            } catch (error) {
                console.error("Error fetching source item:", error);
            }
        }

        fetchSourceItem();
    }, [id]);

    if (!sourceItem) {
        return <div>Loading...</div>;
    }

    const formatDate = (dateString: string) => {
        const date = new Date(dateString);
        return date.toLocaleString(); // Adjust the format according to your requirements
    };

    return ( <div className="cover-background min-vh-100">
        <NavbarInside />
            <Container>
                <div className="row justify-content-center">
                    <div className="col-12 text-center">
                        <p className="text-l mb-0 italic">
                            {sourceItem.category.name}
                        </p>
                        <h1 className="text-3xl font-bold">{sourceItem.title}</h1>
                        <p className="text-xl">{formatDate(sourceItem.createdOn)}</p>

                        <button
                            className="pressed"
                            onClick={() => window.open(sourceItem.urlFile, '_blank')}
                        >
                            Download File
                        </button>


                        <p
                            className="text-xl mt-4"
                            dangerouslySetInnerHTML={{__html: sourceItem.description}}
                        ></p>
                    </div>
                </div>

            </Container>
        </div>
    );
}

export default SourceDetail;
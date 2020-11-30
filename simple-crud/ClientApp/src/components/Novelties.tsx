import React, { useEffect, useState } from 'react';
import { Pagination, PaginationProps } from "./Pagination"
import { Novelty, NoveltyProps } from "./Novelty";
import { Alert } from "./Alert";
import './Novelties.css';

const noveltiesPerPage = 10;
const expandedNovelties = new Set<number>();

export function Novelties() {
    const [currentPageNo, setCurrentPageNo] = useState(1);
    const [novelties, setNovelties] = useState(Array<NoveltyViewModel>(0));
    const [isLoading, setIsLoading] = useState(false);
    const [pagesCount, setPagesCount] = useState(null as number | null)
    const [alertState, setAlertState] = useState({show: false, message: ""});

    useEffect(() => {
        setIsLoading(true);
        Promise.all([loadNoveltiesCount(), loadCurrentPage()]).finally(() => setIsLoading(false));
    }, [currentPageNo])

    function onNoveltyClick(id: number) {
        setNovelties(prev => {
            if (expandedNovelties.has(id)) {
                expandedNovelties.delete(id);
            } else {
                expandedNovelties.add(id);
            }
            
            const novelty = prev.find(x => x.id === id)!;

            novelty.expanded = !novelty.expanded;

            return [...prev];
        });
    }

    async function onDelete(id: number, event: React.MouseEvent) {
        event.stopPropagation();

        await fetch(`novelty/${id}`, {
            method: 'DELETE',
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            }
        })

        setNovelties(prev => {
            const toDeleteIndex = prev.findIndex(x => x.id === id);
            prev.splice(toDeleteIndex, 1);

            return [...prev];
        });

        setAlertState({show: true, message: "Novelty was sucessfully deleted."})
    }

    function getOnModify(novelty: NoveltyViewModel) {
        return (name: string, lastChanged: Date) => {
            setNovelties(prev => {
                const noveltyToChange = prev.find(x => x.id === novelty.id);
                noveltyToChange!.name = name;
                noveltyToChange!.lastChanged = lastChanged;

                return [...prev].sort((a, b) => new Date(b.lastChanged).getTime() - new Date(a.lastChanged).getTime());
            });

            setAlertState({ show: true, message: "Novelty was sucessfully modified." })
        };
    }

    function getPaginationProps(): PaginationProps {
        return {
            pagesCount: pagesCount ?? 0,
            setCurrentPageNo: setCurrentPageNo
        }
    }

    function getNoveltyProps(novelty: NoveltyViewModel) : NoveltyProps {
        return {
            id: novelty.id,
            onChange: getOnModify(novelty)
        }
    }

    async function loadNoveltiesCount() {
        const noveltiesCountResponse = await fetch('noveltyInfo/getcount');
        const noveltiesCount = await noveltiesCountResponse.json();

        const pagesCount = Math.ceil(noveltiesCount / noveltiesPerPage);

        setPagesCount(pagesCount);
    }

    async function loadCurrentPage() {
        const take = encodeURIComponent(noveltiesPerPage);
        const offset = encodeURIComponent(noveltiesPerPage * (currentPageNo - 1));

        const currentPageResponse = await fetch(`noveltyInfo/?take=${take}&offset=${offset}`);
        const currentPage: Array<NoveltyDto> = await currentPageResponse.json();

        const novelties = currentPage.map<NoveltyViewModel>(x => ({ ...x, expanded: expandedNovelties.has(x.id) }));
        setNovelties(novelties);
    }

    function getAlertProps(alertState: {show: boolean, message: string}) {
        return {show: alertState.show, message:alertState.message, hideAfterSeconds: 2, onAlertClose: () => setAlertState(prev => ({...prev, show: false}))};
    }

    return (
        <>
        {
            isLoading ? <div> loadin...</div> :        
        <div className="list-container">
            <Alert {...getAlertProps(alertState)}></Alert>
            <div className="row list-header">
                <div className="col-sm-1">
                    No.
                </div>
                <div className="col-sm-8">
                    Name
                </div>
                <div className="col-sm-3 list-last-col">
                    Last changed
                </div>
            </div>
        <div className="accordion">
            {novelties.map((novelty, index) =>
                <div key={index}>
                    <div className="card">
                        <div className="card-header" id="headingOne">
                            <div className="list-button row" onClick={() => onNoveltyClick(novelty.id)}>
                            <div className="col-sm-1">
                                    {index+1}
                            </div>
                            <div className="col-sm-8">
                                    {novelty.name}
                            </div>
                            <div className="col-sm-3 list-last-col">
                                    {novelty.lastChanged.toLocaleString('en-GB')}
                                    <div className="delete-container" onClick={(event) => onDelete(novelty.id, event)}>
                                        <svg width="1em" height="1em" viewBox="0 0 16 16" className="bi bi-trash-fill" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                            <path fillRule="evenodd" d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z" />
                                        </svg>
                                    </div>
                            </div>
                            </div>
                        </div>
                        <div id="collapseOne" className={novelty.expanded ? "collapse show" : "collapse"}>
                            <div className="card-body">
                                {novelty.expanded ? <Novelty {...getNoveltyProps(novelty)}></Novelty> : <></>}
                            </div>
                        </div>
                    </div>
                </div>
        )}
        </div>
        </div>
        }
        <Pagination {...getPaginationProps()} />
        </>
      );
}


interface NoveltyDetailedDto extends NoveltyDto {
    description: string;
}

interface NoveltyViewModel extends NoveltyDto {
    expanded: boolean;
}

interface NoveltyDto {
    id: number;
    name: string;
    lastChanged: Date;
}
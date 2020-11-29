import React, { useEffect, useState } from 'react';
import { Pagination, PaginationProps } from "./Pagination"
import { Novelty, NoveltyProps } from "./Novelty";
import './Novelties.css';

const noveltiesPerPage = 10;
const expandedNovelties = new Set<number>();

export function Novelties() {
    const [currentPageNo, setCurrentPageNo] = useState(1);
    const [novelties, setNovelties] = useState(Array<NoveltyViewModel>(0));
    const [isLoading, setIsLoading] = useState(false);
    const [pagesCount, setPagesCount] = useState(null as number | null)

    useEffect(() => {
        setIsLoading(true);

        const promise2 = loadNoveltiesCount();
        const promise3 = loadCurrentPage();
        
        Promise.all([promise2, promise3]).finally(() => setIsLoading(false));

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

    function getPaginationProps(): PaginationProps {
        return {
            pagesCount: pagesCount ?? 0,
            setCurrentPageNo: setCurrentPageNo
        }
    }

    function getNoveltyProps(novelty: NoveltyViewModel) : NoveltyProps {
        return {
            id: novelty.id
        }
    }

    async function loadNoveltiesCount() {
        const noveltiesCountResponse = await fetch('novelty/getcount');
        const noveltiesCount = await noveltiesCountResponse.json();

        const pagesCount = Math.ceil(noveltiesCount / noveltiesPerPage);

        setPagesCount(pagesCount);
    }

    async function loadCurrentPage() {
        const take = encodeURIComponent(noveltiesPerPage);
        const offset = encodeURIComponent(noveltiesPerPage * (currentPageNo - 1));

        const currentPageResponse = await fetch(`novelty/?take=${take}&offset=${offset}`);
        const currentPage: Array<NoveltyDto> = await currentPageResponse.json();

        const novelties = currentPage.map<NoveltyViewModel>(x => ({ ...x, expanded: expandedNovelties.has(x.id) }));
        setNovelties(novelties);
    }

    return (
        <>
        {
            isLoading ? <div> loadin...</div> :        
        <div className="list-container">
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
                                    {index}
                            </div>
                            <div className="col-sm-8">
                                    {novelty.name}
                            </div>
                            <div className="col-sm-3 list-last-col">
                                    {novelty.lastChanged.toLocaleString()}
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
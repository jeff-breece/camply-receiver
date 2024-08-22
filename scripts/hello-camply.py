from datetime import datetime
import logging
from typing import List

from camply.containers import AvailableCampsite, SearchWindow
from camply.search import SearchRecreationDotGov

logging.basicConfig(format="%(asctime)s [%(levelname)8s]: %(message)s",
                    level=logging.INFO)

month_of_june = SearchWindow(start_date=datetime(year=2024, month=6, day=1),
                             end_date=datetime(year=2024, month=6, day=30))
camping_finder = SearchRecreationDotGov(search_window=month_of_june,
                                        recreation_area=2725,  # Glacier Ntl Park
                                        weekends_only=False,
                                        nights=1)
matches: List[AvailableCampsite] = camping_finder.get_matching_campsites(log=True, verbose=False,
                                                                         continuous=False)

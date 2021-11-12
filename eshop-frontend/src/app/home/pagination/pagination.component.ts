import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import  paginate from 'jw-paginate';
import { Util } from 'src/app/util/util';
 
@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit,OnChanges {

  @Input() items: Array<any>;
    @Output() changePage = new EventEmitter<any>(true);
    @Input() initialPage = 1;
    @Input() pageSize = Util.pageSize;
    @Input() maxPages = 10;

    pager: any = {};

    ngOnInit() {

        if (this.items && this.items.length) {
            this.setPage(this.initialPage);
          }
    }

    ngOnChanges(changes: SimpleChanges) {

      if (changes.items.currentValue !== changes.items.previousValue) {
        this.setPage(this.initialPage);
      }
    }

    setPage(page: number) {

        this.pager = paginate(this.items.length, page, this.pageSize, this.maxPages);

        // get new page of items from items array
        var pageOfItems = this.items.slice(this.pager.startIndex, this.pager.endIndex + 1);
    
        // call change page function in parent component
        this.changePage.emit(pageOfItems);
    }


}

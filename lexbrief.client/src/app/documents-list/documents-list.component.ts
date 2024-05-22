import {Component, OnInit} from '@angular/core';
import {CommonModule} from "@angular/common";
import { DocumentsListService } from "./documents-list.service";
import { tap } from "rxjs";
import { MaterialModule } from '../material.module';

@Component({
  selector: 'app-documents-list',
  providers: [DocumentsListService],
  imports: [CommonModule, MaterialModule],
  standalone: true,
  templateUrl: './documents-list.component.html'
})
export class DocumentsListComponent implements OnInit {
  documents: any[];

  constructor(private  service: DocumentsListService) {
   this.documents = [];
  }

  ngOnInit(): void {
    this.service.getDocuments().pipe(
      tap((documents) => {
        this.documents = documents;
      })
    ).subscribe();
  }

}

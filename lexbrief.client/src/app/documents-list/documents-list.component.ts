import {Component, OnInit} from '@angular/core';
import {CommonModule} from "@angular/common";
import { DocumentDto, DocumentsListService } from "./documents-list.service";
import { tap } from "rxjs";
import { MaterialModule } from '../material.module';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-documents-list',
  providers: [DocumentsListService],
  imports: [CommonModule, MaterialModule],
  standalone: true,
  templateUrl: './documents-list.component.html',
  styleUrls: ['./documents-list.component.css']
})
export class DocumentsListComponent implements OnInit {
  dataSource!: MatTableDataSource<DocumentDto>;
  //documents: any[];
  displayedColumns: string[] = ['id', 'title'];


  constructor(private  service: DocumentsListService) {
  }

  ngOnInit(): void {
    this.service.getDocuments().pipe(
      tap((documents) => {
        //this.documents = documents;
        this.dataSource = new MatTableDataSource(documents);
      })
    ).subscribe();
  }

  onSelectionChange(event: any) {
    console.log('Righe selezionate:');
  }

}

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from "@angular/common";
import { MaterialModule } from '../material.module';

@Component({
  selector: 'app-sentiment-overview',
  imports: [CommonModule, MaterialModule],
  standalone: true,
  templateUrl: './sentiment-overview.component.html',
  styleUrls: ['./sentiment-overview.component.css']
})
export class SentimentOverviewComponent implements OnInit {
  @Input() set sentimentDto(dto: any) {
  }
  constructor() {
  }

  ngOnInit(): void {

  }

}

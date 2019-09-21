import * as React from "react"
import {v4 as uuid} from "uuid"
import * as M from "materialize-css";

interface DatepickerProps {
  className?: string
  name: string
  cssOptions?: Partial<M.DatepickerOptions>
}

class Datepicker<T extends DatepickerProps> extends React.Component<T, {}> {
  componentDidMount(): void {
    M.Datepicker.init(document.querySelectorAll(".datepicker"), this.props.cssOptions || {})
  }

  render() {
    let id = uuid()
    return (
      <div className={"col input-field " + (this.props.className || "")}>
        <input className="datepicker" type="text" id={id}/>
        <label htmlFor={id}>{this.props.name}</label>
      </div>
    )
  }
}

export default Datepicker

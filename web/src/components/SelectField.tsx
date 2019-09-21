import * as React from "react"
import * as M from "materialize-css";

interface SelectFieldProps {
  options: Map<string, string>
  name: string
  placeholder?: string
  onChange: (value: string) => void
  className?: string
  cssOptions?: Partial<M.FormSelectOptions>
}

class SelectField<T extends SelectFieldProps> extends React.Component<T, {}> {

  componentDidMount(): void {
    M.FormSelect.init(document.querySelectorAll("select"), this.props.cssOptions || {})
  }

  render() {
    let options: any[] = []
    this.props.options.forEach((name, option) => {
      options.push(
        <option value={option}>{name}</option>
      )
    })
    let placeholder = (
      <option value="" disabled selected>{this.props.placeholder}</option>
    )
    return (
      <div className={"input-field col " + (this.props.className || "")}>
        <select onChange={event => this.props.onChange(event.target.value)}>
          {this.props.placeholder ? placeholder : ""}
          {options}
        </select>
        <label>{this.props.name}</label>
      </div>
    )
  }
}

export default SelectField
